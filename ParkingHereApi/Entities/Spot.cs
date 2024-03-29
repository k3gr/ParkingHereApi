﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingHereApi.Entities
{
    public class Spot
    {
        public int Id { get; set; }
        [Column(TypeName = "decimal(18,2)")]

        public decimal Price { get; set; }
        public string Type { get; set; }
        public bool IsAvailable { get; set; }
        public int ParkingId { get; set; }
        public virtual Parking Parking { get; set; }
        public virtual List<Reservation> Reservations { get; set; }
    }
}
