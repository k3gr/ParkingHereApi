using System.ComponentModel.DataAnnotations;

namespace ParkingHereApi.Models
{
    public class CreateSpotDto
    {
        public decimal Price { get; set; }
        public string Type { get; set; }
        public bool IsAvailable { get; set; }
        public int ParkingId { get; set; }
    }
}
