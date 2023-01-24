using DataAccess.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Contracts
{
    public interface IReportsRepository
    {
        Task<IEnumerable<SendCommentConfiguration>> GetAllSendRequests();
        Task<SendCommentConfiguration> GetSendRequestById(int id);
    }
}
