using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDTOs
{
    public class SendCommentConfigurationDetailsDTO
    {
        public WebsiteDTO WebsiteInfo { get; set; }
        public bool Compeleted { get; set; }
        public string CommentType { get; set; }
        public int DestinationCategoryId { get; set; }
        public int CommentGroupId { get; set; }
        public List<int> Ids { get; set; }
        public int ProductPerSendCount { get; set; }
        public int CommentsPerProductCount { get; set; }
        public string Period { get; set; }
        public string WeeklyDOW { get; set; }
        public int? WeeklyHour { get; set; }
        public int? WeeklyMinute { get; set; }
        public int? DailyHour { get; set; }
        public int? DailyMinute { get; set; }
        public int? HourlyMinute { get; set; }
    }
}


