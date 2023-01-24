using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDTOs
{
    public class CronExpressionTimeTableDTO
    {
        public string Period { get; set; }
        public string HourlyMinutes { get; set; }
        public string DailyHours { get; set; }
        public string DailyMinutes { get; set; }
        public string WeeklyDow { get; set; }
        public string WeeklyHours { get; set; }
        public string WeeklyMinutes { get; set; }
    }
}
