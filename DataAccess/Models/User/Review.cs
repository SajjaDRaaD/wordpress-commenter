using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.User
{
    public class Review
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Body { get; set; }
        public int Rating { get; set; }

        public int CommentGroupId { get; set; }
        public CommentGroup CommentGroup { get; set; }
    }

}
