using ClientApp.Models.User;
using DataAccess.Repository.Contracts;

namespace ClientApp.Services.User
{
    public class UDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UDashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DashboardVm> GetDashboardCounts()
        {
            var websites = await _unitOfWork.WebsiteRepository.GetAll();
            var groups = await _unitOfWork.CommentGroupRepository.GetAll();
            var comments = await _unitOfWork.CommentRepository.GetAll();
            var reviews = await _unitOfWork.ReviewRepository.GetAll();
            var sends = await _unitOfWork.ReportRepository.GetAllSendRequests();

            var counts = new DashboardVm
            {
                WebsitesCount = websites.Count(),
                CommentsCount = comments.Count() + reviews.Count(),
                SendsCount = sends.Count(),
                GroupsCount = groups.Count()
            };

            return counts;
        }
    }
}
