using Microsoft.AspNetCore.Mvc;
using ParkingHereApi.Models;

namespace ParkingHereApi.Services
{
    public interface IReservationService
    {
        int Create(int parkingId, int spotId, CreateReservationDto dto);
        void Delete(int id);
        IEnumerable<ReservationDto> GetAll(int parkingId, int spotId);
    }
}