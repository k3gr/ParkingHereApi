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
        IEnumerable<ReservationDto> GetByParkingId(int parkingId);
        IEnumerable<ReservationDto> GetMyReservation();
        IEnumerable<ReservationDto> GetMyPastReservation();
        IEnumerable<ReservationDto> GetAllParkingsCurrentReservation();
        IEnumerable<ReservationDto> GetAllParkingsPastReservation();
    }
}