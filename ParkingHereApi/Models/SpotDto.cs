using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingHereApi.Models
{
    public class SpotDto
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public string Type { get; set; }
        public bool IsAvailable { get; set; }
        //public List<ReservationDto> Reservations { get; set; }
    }
}
