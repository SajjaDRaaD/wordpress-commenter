using AppDTOs;
using AutoMapper;
using DataAccess.Repository.Contracts;
using System.Text.Json;

namespace ClientApp.Services.User
{
    public class UReportsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UReportsService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SendCommentConfigurationDTO>> GetAllSendRequests()
        {
            var result = await _unitOfWork.ReportRepository.GetAllSendRequests();
            var mappedResult = _mapper.Map<IEnumerable<SendCommentConfigurationDTO>>(result);

            return mappedResult;
        }

        public async Task<SendCommentConfigurationDTO> GetSendRequest(int id)
        {
            var result = await _unitOfWork.ReportRepository.GetSendRequestById(id);
            var mappedResult = _mapper.Map<SendCommentConfigurationDTO>(result);
            
            return mappedResult;
        }
    }
}
