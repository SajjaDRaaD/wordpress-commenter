using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Repository.Contracts;

namespace DataAccess.Repository
{
    public class SendCommentRepository : ISendCommentRepository
    {
        public Task<string> CreateSendCommentJob(string comment)
        {
            throw new NotImplementedException();
        }

        public Task DeleteSendCommentJob()
        {
            throw new NotImplementedException();
        }
    }
}
