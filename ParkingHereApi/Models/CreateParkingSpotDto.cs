using System.ComponentModel.DataAnnotations;

namespace ParkingHereApi.Models
{
    public class CreateParkingSpotDto
    {
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public bool IsAvailable { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public int ParkingId { get; set; }

    }
}
