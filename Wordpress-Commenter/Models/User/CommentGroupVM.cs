using AppDTOs;
using DataAccess.Models.User;

namespace ClientApp.Models.User
{
    public class CommentGroupVM
    {
        public List<CommentGroupDTO> CommentGroupsList { get; set; }
        public CommentGroupDTO CommentGroup { get; set; }
    }
}
