using Microsoft.AspNetCore.Mvc;
using ParkingHereApi.Entities;
using ParkingHereApi.Models;

namespace ParkingHereApi.Services
{
    public interface IReservationService
    {
        Reservation Create(int parkingId, CreateReservationDto createReservationDto);
        void Delete(int id);
        IEnumerable<ReservationDto> GetAll(int parkingId, int spotId);
    }
}