using DataAccess.DataContext;
using DataAccess.Models.User;
using DataAccess.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ReportsRepository : IReportsRepository
    {
        private readonly ApplicationDatabaseContext _context;

        public ReportsRepository(ApplicationDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SendCommentConfiguration>> GetAllSendRequests()
        {
            var result = await _context.SendConfigs.ToListAsync();
            return result;
        }

        public async Task<SendCommentConfiguration> GetSendRequestById(int id)
        {
            var result = await _context.SendConfigs.FindAsync(id);
            return result;
        }
    }
}
