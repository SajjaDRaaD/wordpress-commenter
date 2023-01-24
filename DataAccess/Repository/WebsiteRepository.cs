using Microsoft.EntityFrameworkCore;
using DataAccess.DataContext;
using DataAccess.Models.User;
using DataAccess.Repository.Contracts;

namespace DataAccess.Repository
{
    public class WebsiteRepository : IWebsiteRepository
    {

        private readonly ApplicationDatabaseContext _context;
        public WebsiteRepository(ApplicationDatabaseContext context)
        {
            _context = context;
        }

        public bool CheckDuplicateWebsite(string url)
        {
            if (_context.Websites.Any(x => x.Url == url))
            {
                return true;
            }

            return false;
        }

        public async Task Create(Websites website)
        {
            await _context.Websites.AddAsync(website);
        }

        public async Task Delete(int Id)
        {
            var website = await _context.Websites.FindAsync(Id);
            _context.Websites.Remove(website);

        }

        public async Task Edit(Websites website)
        {
            var oldWebsite = await _context.Websites.FindAsync(website.Id);

            oldWebsite.CustomerKey = website.CustomerKey;
            oldWebsite.CustomerSecret = website.CustomerSecret;
            oldWebsite.HasApiAccess = website.HasApiAccess;

        }

        public async Task<Websites> Get(int Id)
        {
            return await _context.Websites.FindAsync(Id);
        }

        public async Task<List<Websites>> GetAll()
        {
            return await _context.Websites.ToListAsync();
        }
    }
}
