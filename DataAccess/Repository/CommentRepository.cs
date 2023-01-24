using Microsoft.EntityFrameworkCore;
using DataAccess.DataContext;
using DataAccess.Models.User;
using DataAccess.Repository.Contracts;

namespace DataAccess.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDatabaseContext _context;

        public CommentRepository(ApplicationDatabaseContext context)
        {
            _context = context;
        }

        public async Task Create(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
        }

        public async Task CreateBulk(List<Comment> comments)
        {
            await _context.Comments.AddRangeAsync(comments);
        }

        public async Task Delete(int Id)
        {
            var comment = await _context.Comments.FindAsync(Id);
            _context.Comments.Remove(comment);
        }

        public async Task<Comment> Get(int Id)
        {
            return await _context.Comments.FindAsync(Id);
        }

        public async Task<List<Comment>> GetAll()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<List<Comment>> GetCommentsByGroupId(int id)
        {
            return await _context.Comments.Where(x => x.CommentGroupId == id).ToListAsync();
        }
    }
}
