using DataAccess.Models.User;
using AppDTOs;
using RestSharp;
using RestSharp.Authenticators;
using ClientApp.Models.User;
using ClientApp.Services.Rest;
using DataAccess.Repository.Contracts;

namespace ClientApp.Services.User
{
    public class UWebsitesService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IRestClientService _restClient;

        public UWebsitesService(IRestClientService restClient, IUnitOfWork unitOfWork)
        {
            _restClient = restClient;
            _unitOfWork = unitOfWork;
        }

        public async Task<MessageDTO> AddWebsite(Websites websites)
        {
            CheckSlashEndOfUrl(websites);

            if (CheckWebsiteDuplicate(websites))
            {

                return new MessageDTO
                {
                    Message = $"وبسایت {websites.Name} قبلا ثبت شده است.",
                    Status = "error",
                };

            }

            if (CheckInputsEmpty(websites))
            {
                return new MessageDTO
                {
                    Message = "نام وبسایت و آدرس وبسایت الزامی است.",
                    Status = "error",
                };
            }

            if (!await IsWordpress(websites))
            {
                return new MessageDTO
                {
                    Message = "خطایی در ارتباط با وبسایت شما رخ داده است، لطفا از درست بودن آدرس اطمینان حاصل کنید.",
                    Status = "error",
                };
            }

            if (WooAuthInputsNotEmpty(websites))
            {

                var apiAccess = await _restClient.CheckApiAccess(websites);

                if (apiAccess)
                {

                    websites.HasApiAccess = true;
                    await _unitOfWork.WebsiteRepository.Create(websites);
                    await _unitOfWork.SaveChanges();

                    return new MessageDTO
                    {
                        Message = $"وبسایت {websites.Name} با موفقیت ثبت شد.",
                        Status = "success"
                    };

                }
                else
                {

                    return new MessageDTO
                    {
                        Message = $"اطلاعات مربوط به کلید مصرف کننده و رمز مصرف کننده معتبر نمی باشد، لطفا مجددا تلاش کنید",
                        Status = "error"
                    };

                }

            }
            else
            {
                if (websites.CustomerKey == null ^ websites.CustomerSecret == null)
                {

                    return new MessageDTO
                    {
                        Message = $"اطلاعات مربوط به کلید مصرف کننده و رمز مصرف کننده معتبر نمی باشد، لطفا مجددا تلاش کنید",
                        Status = "error"
                    };

                }
                else
                {

                    websites.HasApiAccess = false;
                    await _unitOfWork.WebsiteRepository.Create(websites);
                    await _unitOfWork.SaveChanges();

                    return new MessageDTO
                    {
                        Message = $"وبسایت {websites.Name} با موفقیت ثبت شد.",
                        Status = "success"
                    };

                }

            }
        }

        private async Task<bool> IsWordpress(Websites websites)
        {
            return await _restClient.IsWordpress(websites);
        }

        private static bool WooAuthInputsNotEmpty(Websites websites)
        {
            return websites.CustomerKey != null && websites.CustomerSecret != null;
        }

        private bool CheckWebsiteDuplicate(Websites websites)
        {
            return _unitOfWork.WebsiteRepository.CheckDuplicateWebsite(websites.Url);
        }

        private static bool CheckInputsEmpty(Websites websites)
        {
            return websites.Url == null || websites.Name == null;
        }

        private static void CheckSlashEndOfUrl(Websites websites)
        {
            if (!websites.Url.EndsWith("/"))
            {
                websites.Url = websites.Url + "/";
            }
        }

        public async Task<bool> DeleteWebsite(int id)
        {
            var configs = _unitOfWork.SendCommentConfigurationRepository.GetByWebsiteId(id);

            if (configs.Count() == 0)
            {
                await _unitOfWork.WebsiteRepository.Delete(id);
                await _unitOfWork.SaveChanges();
                return true;
            }

            return false;
            
        }

        public async Task<MessageDTO> EditWebsite(Websites website)
        {
            if (website.CustomerKey == null ^ website.CustomerSecret == null )
            {
                return new MessageDTO
                {
                    Message = "کلید مصرف کننده و رمز مصرف کننده الزامی است.",
                    Status = "error",
                };
            }

            if (await _restClient.CheckApiAccess(website))
            {
                website.HasApiAccess = true;
                await _unitOfWork.WebsiteRepository.Edit(website);
                await _unitOfWork.SaveChanges();
                return new MessageDTO
                {
                    Message = $"اطلاعات وبسایت {website.Name} با موفقیت ویرایش شد.",
                    Status = "success"
                };
            }

            return new MessageDTO
            {
                Message = "کلید مصرف کننده یا رمز مصرف کننده معتبر نمی باشد",
                Status = "error"
            };
        }

        public async Task<List<Websites>> GetAllWebsites()
        {
            return await _unitOfWork.WebsiteRepository.GetAll();
        }

        public async Task<Websites> GetWebsite(int Id)
        {
            return await _unitOfWork.WebsiteRepository.Get(Id);
        }
    }
}
