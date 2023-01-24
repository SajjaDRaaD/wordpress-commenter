using DataAccess.Models.User;
using DataAccess.Repository.Contracts;

namespace ClientApp.Services.User
{
    public class UCommentGroupService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UCommentGroupService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddCommentGroup(CommentGroup commentGroup)
        {
            await _unitOfWork.CommentGroupRepository.Create(commentGroup);
            await _unitOfWork.SaveChanges();
        }

        public async Task EditCommentGroup(CommentGroup commentGroup) {
            await _unitOfWork.CommentGroupRepository.Edit(commentGroup);
            await _unitOfWork.SaveChanges();
        }
        public async Task<bool> DeleteCommentGroup(int Id)
        {
            var configs = _unitOfWork.SendCommentConfigurationRepository.GetByGroupId(Id);

            if (configs.Count() == 0)
            {
                await _unitOfWork.CommentGroupRepository.Delete(Id);
                await _unitOfWork.SaveChanges();
                return true;
            }

            return false;
        }
        public async Task<CommentGroup> GetCommentGroup(int Id)
        {
            return await _unitOfWork.CommentGroupRepository.Get(Id);
        }
        public async Task<List<CommentGroup>> GetAllCommentGroups()
        {
            return await _unitOfWork.CommentGroupRepository.GetAll();
        }
    }
}
