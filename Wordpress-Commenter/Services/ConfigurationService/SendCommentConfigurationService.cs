using AppDTOs;
using AutoMapper;
using ClientApp.Services.User;
using DataAccess.Models.User;
using DataAccess.Repository.Contracts;
using System.Text.Json;

namespace ClientApp.Services.ConfigurationService
{
    public class SendCommentConfigurationService
    {
        private readonly UWebsitesService _uWebsitesService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SendCommentConfigurationService(UWebsitesService uWebsitesService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _uWebsitesService = uWebsitesService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<SendCommentConfiguration> PrepareConfiguration(IFormCollection form)
        {
            var website = await _uWebsitesService.GetWebsite(Convert.ToInt32(form["websiteId"]));
            var mappedWebsite = _mapper.Map<WebsiteDTO>(website);
            var postIds = Convert.ToString(form["posts-id"]);
            var config = new SendCommentConfigurationDetailsDTO
            {
                WebsiteInfo = mappedWebsite,
                Compeleted = false,
                CommentType = form["comment-type"],
                CommentGroupId = Convert.ToInt32(form["commentCatId"]),
                DestinationCategoryId = Convert.ToInt32(form["desCatId"]),
                CommentsPerProductCount = Convert.ToInt32(form["commentsPerProductCount"]),
                ProductPerSendCount = Convert.ToInt32(form["productsPerSendCount"]),
                Ids = postIds.Split(",").Select(int.Parse).ToList(),
                Period = form["sendPeriod"],
                WeeklyDOW = string.IsNullOrEmpty(form["weeklyDOW"]) ? "" : form["weeklyDOW"],
                WeeklyHour = string.IsNullOrEmpty(form["weeklyHour"]) ? null : Convert.ToInt32(form["weeklyHour"]),
                WeeklyMinute = string.IsNullOrEmpty(form["weeklyMinute"]) ? null : Convert.ToInt32(form["weeklyMinute"]),
                DailyHour = string.IsNullOrEmpty(form["dailyHour"]) ? null : Convert.ToInt32(form["dailyHour"]),
                DailyMinute = string.IsNullOrEmpty(form["dailyMinute"]) ? null : Convert.ToInt32(form["dailyMinute"]),
                HourlyMinute = string.IsNullOrEmpty(form["hourlyMinute"]) ? null : Convert.ToInt32(form["hourlyMinute"])
            };

            var configJson = JsonSerializer.Serialize(config);
            var configModel = new SendCommentConfigurationDTO
            {
                SendId = Guid.NewGuid(),
                Configuration = configJson
            };
            var mappedConfigModel = _mapper.Map<SendCommentConfiguration>(configModel);
            var result = await _unitOfWork.SendCommentConfigurationRepository.Create(mappedConfigModel);

            return result;
        }

    }
}
