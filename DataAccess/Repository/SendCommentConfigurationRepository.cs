using DataAccess.DataContext;
using DataAccess.Models.User;
using DataAccess.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class SendCommentConfigurationRepository : ISendCommentConfigurationRepository
    {
        private readonly ApplicationDatabaseContext _context;
        public SendCommentConfigurationRepository(ApplicationDatabaseContext context)
        {
            _context = context;
        }

        public async Task<SendCommentConfiguration> Create(SendCommentConfiguration config)
        {
            await _context.SendConfigs.AddAsync(config);
            return config;
        }

        public async Task Delete(int id)
        {
            var config = await _context.SendConfigs.FindAsync(id);
            _context.SendConfigs.Remove(config);
        }

        public async Task EditById(SendCommentConfiguration config)
        {
            var oldConfig = _context.SendConfigs.SingleOrDefault(x=> x.Id == config.Id);
            oldConfig.Configuration = config.Configuration;
        }

        public async Task EditBySendId(SendCommentConfiguration config)
        {
            var oldConfig = _context.SendConfigs.SingleOrDefault(x => x.SendId == config.SendId);
            oldConfig.Configuration = config.Configuration;
        }

        public IEnumerable<SendCommentConfiguration> GetByGroupId(int groupId)
        {
            var configs = _context.SendConfigs.FromSqlRaw($"SELECT * FROM SendConfigs WHERE JSON_VALUE(Configuration,'$.CommentGroupId') = {groupId}").ToList();
            return configs;
        }

        public async Task<SendCommentConfiguration> GetById(int id)
        {
            return await _context.SendConfigs.FindAsync(id);
        }

        public async Task<SendCommentConfiguration> GetBySendId(Guid sendId)
        {
            return await _context.SendConfigs.Where(x => x.SendId == sendId).FirstOrDefaultAsync();
        }

        public IEnumerable<SendCommentConfiguration> GetByWebsiteId(int websiteId)
        {
            var configs = _context.SendConfigs.FromSqlRaw($"SELECT * FROM SendConfigs WHERE JSON_VALUE(Configuration,'$.WebsiteInfo.Id') = {websiteId}").ToList();
            return configs;
        }
    }
}
