using AppDTOs;

namespace ClientApp.Models.User
{
    public class SendCommentVm
    {
        public List<WebsiteDTO> WebsitesList { get; set; }
        public List<CommentGroupDTO> CommentGroupList { get; set; }
    }
}
