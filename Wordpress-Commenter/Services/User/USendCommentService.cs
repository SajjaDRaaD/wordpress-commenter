using AppDTOs;
using AutoMapper;
using ClientApp.Services.ConfigurationService;
using ClientApp.Services.Rest;
using DataAccess.Models.User;
using DataAccess.Repository.Contracts;
using Hangfire;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System.Xml.Linq;
using System.Text.Json;

namespace ClientApp.Services.User
{
    public class USendCommentService
    {
        private readonly IRestClientService _restClientService;
        private readonly IMapper _mapper;
        private readonly UWebsitesService _uWebsitesService;
        private readonly UCommentsService _uCommentService;
        private readonly SendCommentConfigurationService _sendCommentConfigurationService;
        private readonly IUnitOfWork _unitOfWork;
        public USendCommentService(IRestClientService restClientService, UWebsitesService uWebsitesService, UCommentsService uCommentService, IMapper mapper, IUnitOfWork unitOfWork, SendCommentConfigurationService sendCommentConfigurationService)
        {
            _restClientService = restClientService;
            _uWebsitesService = uWebsitesService;
            _uCommentService = uCommentService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _sendCommentConfigurationService = sendCommentConfigurationService;
        }

        public async Task<MessageDTO> CreateSendRecurringJob(IFormCollection data)
        {
            try
            {
                var config = await _sendCommentConfigurationService.PrepareConfiguration(data);
                var timeTable = new CronExpressionTimeTableDTO
                {
                    Period = data["sendPeriod"],
                    HourlyMinutes = data["hourlyMinute"],
                    DailyHours = data["dailyHour"],
                    DailyMinutes = data["dailyMinute"],
                    WeeklyDow = data["weeklyDOW"],
                    WeeklyHours = data["weeklyHour"],
                    WeeklyMinutes = data["weeklyMinute"]
                };
                var cronExp = GenerateCronExpression(timeTable);
                RecurringJob.AddOrUpdate($"Send-id :{config.SendId}", () => SendCommentJob(config), cronExp, TimeZoneInfo.Local);
                await _unitOfWork.SaveChanges();

                return new MessageDTO
                {
                    Message = "ثبت ارسال با موفقیت انجام شد",
                    Status = "Success"
                };
            }
            catch (Exception)
            {

                return new MessageDTO
                {
                    Message = "در ثبت ارسال خطایی به وجود آمده",
                    Status = "Faild"
                };
            }
        }

        public async Task<MessageDTO> SendCommentJob(SendCommentConfiguration config)
        {
            // Get Config from Db
            var updatedConfig = await _unitOfWork.SendCommentConfigurationRepository.GetBySendId(config.SendId);
            var ConfigDetails = new SendCommentConfigurationDetailsDTO();

            if (updatedConfig == null)
            {
                ConfigDetails = JsonConvert.DeserializeObject<SendCommentConfigurationDetailsDTO>(config.Configuration);
            } else
            {
                ConfigDetails = JsonConvert.DeserializeObject<SendCommentConfigurationDetailsDTO>(updatedConfig.Configuration);
            }


            //Map Website DTO to Website Entity
            var mappedWebsite = _mapper.Map<Websites>(ConfigDetails.WebsiteInfo);

            //Declare Logical and temp vars
            var tempIds = new List<int> { };
            var productPerSendCount = (ConfigDetails.ProductPerSendCount > ConfigDetails.Ids.Count) ? ConfigDetails.Ids.Count : ConfigDetails.ProductPerSendCount;

            //Send Comment Logic
            if (ConfigDetails.CommentType == "blog")
            {
                var comments = await _uCommentService.GetCommentsByGroupId(ConfigDetails.CommentGroupId);
                var commentCount = 0;


                for (int postCount = 0; postCount < productPerSendCount; postCount++)
                {
                    var postId = ConfigDetails.Ids[postCount];

                        while (commentCount < ConfigDetails.CommentsPerProductCount)
                        {
                            var random = new Random();
                            var index = random.Next(comments.Count);

                            Thread.Sleep(3000);

                            var result = await _restClientService.SendComment(mappedWebsite, comments[index], postId);

                            while (result.Status == "BadRequest")
                            {
                                result = await _restClientService.SendComment(mappedWebsite, comments[index], postId);
                            }

                            if (result.Status == "Created")
                            {
                                Console.WriteLine($"Comment Index ({index}) at Id ({postId}) submited successfuly...");
                                commentCount++;
                            }
                            else
                            {
                                Console.WriteLine($"{postId} - {result.Status} - {result.Message}");
                            }
                        }
                    tempIds.Add(postId);
                    commentCount = 0;
                }

            } else if(ConfigDetails.CommentType == "commerce")
            {

                var reviews = await _unitOfWork.ReviewRepository.GetReviewsByGroupId(ConfigDetails.CommentGroupId);
                var reviewCount = 0;

                for (int productCount = 0; productCount < productPerSendCount; productCount++)
                {
                    var productId = ConfigDetails.Ids[productCount];

                    while (reviewCount < ConfigDetails.CommentsPerProductCount)
                    {
                        var random = new Random();
                        var index = random.Next(reviews.Count);

                        Thread.Sleep(3000);

                        var result = await _restClientService.SendReview(mappedWebsite, reviews[index], productId);

                        while (result.Status == "BadRequest")
                        {
                            result = await _restClientService.SendReview(mappedWebsite, reviews[index], productId);
                        }

                        if (result.Status == "Created")
                        {
                            Console.WriteLine($"Comment Index ({index}) at Id ({productId}) submited successfuly...");
                            reviewCount++;
                        }
                        else
                        {
                            Console.WriteLine($"{productId} - {result.Status} - {result.Message}");
                        }
                    }
                    tempIds.Add(productId);
                    reviewCount = 0;
                }

            }

            //Update Ids in config details
            ConfigDetails.Ids = ConfigDetails.Ids.Except(tempIds).ToList();
            var newConfig = new SendCommentConfiguration
            {
                SendId = config.SendId,
                Configuration = JsonConvert.SerializeObject(ConfigDetails)
            };
            await _unitOfWork.SendCommentConfigurationRepository.EditBySendId(newConfig);
            await _unitOfWork.SaveChanges();

            // Finish Send Job Condition
            if (ConfigDetails.Ids.Count == 0)
            {
                RecurringJob.RemoveIfExists($"Send-id :{config.Id}");

                ConfigDetails.Compeleted = true;

                var finalConfig = new SendCommentConfiguration
                {
                    SendId = config.SendId,
                    Configuration = JsonConvert.SerializeObject(ConfigDetails)
                };

                await _unitOfWork.SendCommentConfigurationRepository.EditBySendId(finalConfig);
                await _unitOfWork.SaveChanges();

                return new MessageDTO
                {
                    Status = "Finished",
                    Message = "عملیات با موفقیت به پایان رسید"
                };
            }

            //Return Response
            return new MessageDTO
            {
                Status = "Success",
                Message = "عملیات با موفقیت انجام شد"
            };
        }

        public async Task<List<CategoryDTO>> GetWebsiteCategories(int websiteId, string commentType)
        {
            var website = await _uWebsitesService.GetWebsite(websiteId);
            var uri = $"{website.Url}/wp-json/wp/v2/categories?per_page=100";
            var client = new RestClient(website.Url);

            if (commentType == "commerce")
            {
                client.Authenticator = OAuth1Authenticator.ForRequestToken(website.CustomerKey, website.CustomerSecret);
                uri = $"{website.Url}/wp-json/wc/v3/products/categories?per_page=100";
            }

            var request = new RestRequest(uri);
            var response = await client.ExecuteAsync(request);
            var pages = Convert.ToInt32(response.Headers.ToList().Find(x => x.Name.ToLower() == "x-wp-totalpages").Value);
            var categories = JsonConvert.DeserializeObject<List<CategoryDTO>>(response.Content);

            if (pages > 1)
            {
                for (int i = 1; i < pages; i++)
                {
                    var paginationRequest = new RestRequest(uri + $"&page={i + 1}");
                    var paginationResponse = await client.ExecuteAsync(paginationRequest);
                    var result = JsonConvert.DeserializeObject<List<CategoryDTO>>(paginationResponse.Content);
                    result.ForEach(category =>
                    {
                        categories.Add(new CategoryDTO
                        {
                            Id = category.Id,
                            Name = category.Name
                        });
                    });
                }

                return categories;
            }
            else
            {
                return categories;
            }
        }

        public async Task<List<int>> GetWordpressPostsIds(int websiteId, string commentType, int catId)
        {
            var website = await _uWebsitesService.GetWebsite(websiteId);
            var uri = $"{website.Url}/wp-json/wp/v2/posts?categories={catId}&per_page=100";
            var client = new RestClient(website.Url);
            var Ids = new List<int>();

            if (commentType == "commerce")
            {
                client.Authenticator = OAuth1Authenticator.ForRequestToken(website.CustomerKey, website.CustomerSecret);
                uri = $"{website.Url}/wp-json/wc/v3/products?category={catId}&per_page=100";
            }

            var request = new RestRequest(uri);
            var response = await client.ExecuteAsync(request);
            var pages = Convert.ToInt32(response.Headers.ToList().Find(x => x.Name.ToLower() == "x-wp-totalpages").Value);
            var parsedJson = JArray.Parse(response.Content);

            foreach (var item in parsedJson)
            {
                Ids.Add(Convert.ToInt32(item["id"]));
            }

            if (pages > 1)
            {
                for (int i = 1; i < pages; i++)
                {
                    var paginationRequest = new RestRequest(uri + $"&page={i + 1}");
                    var paginationResponse = await client.ExecuteAsync(paginationRequest);
                    var paginationparsedJson = JArray.Parse(response.Content);

                    foreach (var item in paginationparsedJson)
                    {
                        Ids.Add(Convert.ToInt32(item["id"]));
                    }
                }

                return Ids;
            }
            else
            {
                return Ids;
            }
        }

        public string GenerateCronExpression(CronExpressionTimeTableDTO timeTable)
        {
            var cronExp = "";

            switch (timeTable.Period)
            {
                case "weekly":

                    if (string.IsNullOrEmpty(timeTable.WeeklyDow) || string.IsNullOrEmpty(timeTable.WeeklyHours) || string.IsNullOrEmpty(timeTable.WeeklyMinutes))
                    {
                        cronExp = "";
                    } else
                    {
                        cronExp = $"0 {timeTable.WeeklyMinutes} {timeTable.WeeklyHours} * * {timeTable.WeeklyDow}";
                    }

                    break;

                case "daily":

                    if (string.IsNullOrEmpty(timeTable.DailyHours) || string.IsNullOrEmpty(timeTable.DailyMinutes))
                    {
                        cronExp = "";
                    }else
                    {
                        cronExp = $"0 {timeTable.DailyMinutes} {timeTable.DailyHours} * * *";
                    }

                    break;

                case "hourly":

                    if (string.IsNullOrEmpty(timeTable.HourlyMinutes))
                    {
                        cronExp = "";
                    }else
                    {
                        cronExp = $"0 {timeTable.HourlyMinutes} * * * *";
                    }

                    break;
            }

            return cronExp;

        }


    }

}
