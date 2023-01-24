

using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models.User
{
    public class CommentGroup
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        public List<Comment> Comment { get; set; }
        public List<Review> Review { get; set; }
    }
}
