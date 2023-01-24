using DataAccess.Models.User;
using DataAccess.Repository.Contracts;

namespace ClientApp.Services.User
{
    public class UReviewService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddReview(Review review)
        {
            await _unitOfWork.ReviewRepository.Create(review);
            await _unitOfWork.SaveChanges();
        }
        
        public async Task AddBulkAsync(List<Review> reviews)
        {
            await _unitOfWork.ReviewRepository.CreateBulk(reviews);
            await _unitOfWork.SaveChanges();
        }

        public async Task DeleteAllByGroup(int groupId)
        {
            await _unitOfWork.ReviewRepository.DeleteAllByGroup(groupId);
            await _unitOfWork.SaveChanges();
        }

        public async Task DeleteReview(int Id)
        {
            await _unitOfWork.ReviewRepository.Delete(Id);
            await _unitOfWork.SaveChanges();
        }
        

        public async Task<Review> GetReview(int Id)
        {
            return await _unitOfWork.ReviewRepository.Get(Id);
        }

        public async Task<List<Review>> GetAllReview()
        {
            return await _unitOfWork.ReviewRepository.GetAll();
        }

        public async Task<List<Review>> GetReviewsByGroupId(int id)
        {
            return await _unitOfWork.ReviewRepository.GetReviewsByGroupId(id);
        }
    }
}
