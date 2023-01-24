using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDTOs
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Body { get; set; }
        public int Rating { get; set; }
        public int CommentGroupId { get; set; }
    }
}
