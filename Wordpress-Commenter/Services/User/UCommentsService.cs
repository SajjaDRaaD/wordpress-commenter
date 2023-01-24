using DataAccess.Models.User;
using DataAccess.Repository.Contracts;

namespace ClientApp.Services.User
{
    public class UCommentsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UCommentsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddComment(Comment comment)
        {
            await _unitOfWork.CommentRepository.Create(comment);
            await _unitOfWork.SaveChanges();
        }

        public async Task AddBulkComments(List<Comment> comments)
        {
            await _unitOfWork.CommentRepository.CreateBulk(comments);
            await _unitOfWork.SaveChanges();
        }
        

        public async Task<Comment> GetComment(int Id)
        {
            return await _unitOfWork.CommentRepository.Get(Id);
        }

        public async Task<List<Comment>> GetAllComments()
        {
            return await _unitOfWork.CommentRepository.GetAll();
        }

        public async Task<List<Comment>> GetCommentsByGroupId(int id)
        {
            return await _unitOfWork.CommentRepository.GetCommentsByGroupId(id);
        }

        public async Task DeleteComment(int Id)
        {
            await _unitOfWork.CommentRepository.Delete(Id);
            await _unitOfWork.SaveChanges();
        }
    }
}
