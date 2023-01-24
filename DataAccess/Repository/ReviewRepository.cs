using Microsoft.EntityFrameworkCore;
using DataAccess.DataContext;
using DataAccess.Models.User;
using System.Diagnostics;
using System.Text.RegularExpressions;
using DataAccess.Repository.Contracts;

namespace DataAccess.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDatabaseContext _context;

        public ReviewRepository(ApplicationDatabaseContext context)
        {
            _context = context;
        }

        public async Task Create(Review review)
        {
            await _context.Reviews.AddAsync(review);
        }

        public async Task CreateBulk(List<Review> reviews)
        {

            await _context.Reviews.AddRangeAsync(reviews);

        }

        public async Task Delete(int Id)
        {
            var review = await _context.Reviews.FindAsync(Id);
            _context.Reviews.Remove(review);

        }

        public async Task DeleteAllByGroup(int groupId)
        {
            List<Review> reviews = _context.Reviews.Where(x => x.CommentGroupId == groupId).ToList();
            _context.Reviews.RemoveRange(reviews);

        }

        public async Task<Review> Get(int Id)
        {
            return await _context.Reviews.FindAsync(Id);
        }

        public async Task<List<Review>> GetAll()
        {
            return await _context.Reviews.ToListAsync();
        }

        public async Task<List<Review>> GetReviewsByGroupId(int id)
        {
            return await _context.Reviews.Where(x => x.CommentGroupId == id).ToListAsync();

        }
    }
}
