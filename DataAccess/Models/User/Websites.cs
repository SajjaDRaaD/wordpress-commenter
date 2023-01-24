using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models.User
{
    public class Websites
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Url { get; set; }
        public string? CustomerKey { get; set; }
        public string? CustomerSecret { get; set; }
        public bool HasApiAccess { get; set; }
        public bool IsActive { get; set; }
    }
}
