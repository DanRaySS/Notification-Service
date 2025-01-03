using System.ComponentModel.DataAnnotations;

namespace API_Service.DTOs
{
    public class CreateNotificationDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string TextContent { get; set; }
        [Required]
        public string Address { get; set; }
    }
}