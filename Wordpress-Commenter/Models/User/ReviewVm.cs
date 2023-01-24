using AppDTOs;

namespace ClientApp.Models.User
{
    public class ReviewVm
    {
        public ReviewDTO Review { get; set; }
        public CommentDTO Comment { get; set; }

        public List<ReviewDTO> Reviews { get; set; }
        public List<CommentDTO> Comments { get; set; }
        public List<CommentGroupDTO> Groups { get; set; }
    }
}
