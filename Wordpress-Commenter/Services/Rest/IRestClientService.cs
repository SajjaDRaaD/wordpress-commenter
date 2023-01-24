using AppDTOs;
using DataAccess.Models.User;
using RestSharp;

namespace ClientApp.Services.Rest
{
    public interface IRestClientService
    {
        Task<MessageDTO> SendComment(Websites website , Comment comment, int postId);
        Task<MessageDTO> SendReview(Websites website, Review review, int productId);
        Task<bool> CheckApiAccess(Websites website);
        Task<bool> IsWordpress(Websites website);
        Task<bool> IsPostExist(int postId, Websites website);
        Task<bool> IsProductExist(int productId, Websites website);
    }
}
