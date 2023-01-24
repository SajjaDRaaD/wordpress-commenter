using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Contracts
{
    public interface ISendCommentRepository
    {
        Task<string> CreateSendCommentJob(string comment);
        Task DeleteSendCommentJob();
    }
}
