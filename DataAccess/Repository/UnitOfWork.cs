using DataAccess.DataContext;
using DataAccess.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ICommentGroupRepository _commentGroupRepository;
        private ICommentRepository _commentRepository;
        private IReportsRepository _reportRepository;
        private IReviewRepository _reviewRepository;
        private ISendCommentConfigurationRepository _sendCommentConfigurationRepository;
        private IWebsiteRepository _websiteRepository;
        private readonly ApplicationDatabaseContext _context;

        public UnitOfWork(ApplicationDatabaseContext context)
        {
            _context = context;
        }

        public ICommentGroupRepository CommentGroupRepository => _commentGroupRepository ??= new CommentGroupRepository(_context);
        public ICommentRepository CommentRepository => _commentRepository ??= new CommentRepository(_context);
        public IReportsRepository ReportRepository => _reportRepository ??= new ReportsRepository(_context);
        public IReviewRepository ReviewRepository => _reviewRepository ??= new ReviewRepository(_context);
        public ISendCommentConfigurationRepository SendCommentConfigurationRepository => _sendCommentConfigurationRepository ??= new SendCommentConfigurationRepository(_context);
        public IWebsiteRepository WebsiteRepository => _websiteRepository ??= new WebsiteRepository(_context);

        
        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
