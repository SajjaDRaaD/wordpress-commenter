using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        ICommentGroupRepository CommentGroupRepository { get; }
        ICommentRepository CommentRepository { get; }
        IReportsRepository ReportRepository { get; }
        IReviewRepository ReviewRepository { get; }
        ISendCommentConfigurationRepository SendCommentConfigurationRepository { get; }
        IWebsiteRepository WebsiteRepository { get; }
        Task SaveChanges();
    }
}
