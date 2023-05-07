using System.ComponentModel.DataAnnotations;

namespace ParkingHereApi.Models
{
    public class UpdateParkingDto
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string ContactEmail { get; set; }
        public string ContactNumber { get; set; }
    }
}
