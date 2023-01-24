using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models.User
{
    public class Comment
    {
        public int Id { get; set; }
        public string Author { get; set; }
        [MaxLength(500)]
        public string Body { get; set; }

        public int CommentGroupId { get; set; }
        public CommentGroup CommentGroup { get; set; }
    }
}
