using DataAccess.Models.User;

namespace DataAccess.Repository.Contracts
{
    public interface ICommentRepository
    {
        Task Create(Comment comment);
        Task CreateBulk(List<Comment> comments);
        Task Delete(int Id);
        Task<Comment> Get(int Id);
        Task<List<Comment>> GetAll();
        Task<List<Comment>> GetCommentsByGroupId(int id);
    }
}
