using DataAccess.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Contracts
{
    public interface ISendCommentConfigurationRepository
    {
        Task<SendCommentConfiguration> Create(SendCommentConfiguration config);
        Task EditById(SendCommentConfiguration config);
        Task EditBySendId(SendCommentConfiguration config);
        Task Delete(int id);
        Task<SendCommentConfiguration> GetById(int id);
        Task<SendCommentConfiguration> GetBySendId(Guid sendId);
        IEnumerable<SendCommentConfiguration> GetByGroupId(int groupId);
        IEnumerable<SendCommentConfiguration> GetByWebsiteId(int websiteId);
    }
}
