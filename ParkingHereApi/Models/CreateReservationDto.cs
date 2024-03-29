﻿using ParkingHereApi.Entities;
using ParkingHereApi.Enums;

namespace ParkingHereApi.Models
{
    public class CreateReservationDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ParkingId { get; set; }
        public string Type { get; set; }
    }
}
