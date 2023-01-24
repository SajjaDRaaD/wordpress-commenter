using Microsoft.EntityFrameworkCore;
using DataAccess.DataContext;
using DataAccess.Models.User;
using DataAccess.Repository.Contracts;

namespace DataAccess.Repository
{
    public class CommentGroupRepository : ICommentGroupRepository
    {
        private readonly ApplicationDatabaseContext _context;

        public CommentGroupRepository(ApplicationDatabaseContext context)
        {
            _context = context;
        }

        public async Task Create(CommentGroup commentGroup)
        {
            await _context.CommentGroup.AddAsync(commentGroup);
        }

        public async Task Delete(int Id)
        {
            var commentGroup = await _context.CommentGroup.FindAsync(Id);
            _context.CommentGroup.Remove(commentGroup);
        }

        public async Task Edit(CommentGroup commentGroup)
        {
            var oldCommentGroup = await _context.CommentGroup.FindAsync(commentGroup.Id);
            oldCommentGroup.Name = commentGroup.Name;
            oldCommentGroup.Type = commentGroup.Type;
        }

        public async Task<CommentGroup> Get(int Id)
        {
            return await _context.CommentGroup.FindAsync(Id);
        }

        public async Task<List<CommentGroup>> GetAll()
        {
            return await _context.CommentGroup.ToListAsync();
        }
    }
}
