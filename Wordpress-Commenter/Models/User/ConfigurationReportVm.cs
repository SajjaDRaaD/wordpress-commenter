using AppDTOs;

namespace ClientApp.Models.User
{
    public class ConfigurationReportVm
    {
        public int Id { get; set; }
        public SendCommentConfigurationDetailsDTO config { get; set; }
        public string Status { get; set; }
    }
}
