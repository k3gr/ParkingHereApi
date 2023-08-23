using System.ComponentModel.DataAnnotations;

namespace ParkingHereApi.Models
{
    public class CreateSpotDto
    {
        public decimal Price { get; set; }
        public string Type { get; set; }
    }
}
