﻿using Microsoft.AspNetCore.Mvc;
using ParkingHereApi.Models;

namespace ParkingHereApi.Services
{
    public interface IParkingService
    {
        IEnumerable<ParkingDto> GetAll();
        int Create(CreateParkingDto dto);
        ParkingDto GetById(int id);
        void Delete(int id);
        void Update(int id, UpdateParkingDto dto);

    }
}
