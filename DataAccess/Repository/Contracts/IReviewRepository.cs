using DataAccess.Models.User;

namespace DataAccess.Repository.Contracts
{
    public interface IReviewRepository
    {
        Task Create(Review review);
        Task CreateBulk(List<Review> reviews);
        Task DeleteAllByGroup(int groupId);
        Task Delete(int Id);
        Task<Review> Get(int Id);
        Task<List<Review>> GetAll();
        Task<List<Review>> GetReviewsByGroupId(int id);
    }
}
