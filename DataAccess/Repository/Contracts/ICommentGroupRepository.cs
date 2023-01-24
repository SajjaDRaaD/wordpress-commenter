using DataAccess.Models.User;

namespace DataAccess.Repository.Contracts
{
    public interface ICommentGroupRepository
    {

        Task Create(CommentGroup commentGroup);
        Task Edit(CommentGroup commentGroup);
        Task Delete(int Id);
        Task<CommentGroup> Get(int Id);
        Task<List<CommentGroup>> GetAll();

    }
}
