using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Author { get; set; }
        [MaxLength(500)]
        public string Body { get; set; }
        public int CommentGroupId { get; set; }
    }
}
