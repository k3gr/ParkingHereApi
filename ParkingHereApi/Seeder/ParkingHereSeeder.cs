﻿using Microsoft.EntityFrameworkCore;
using ParkingHereApi.Entities;
using ParkingHereApi.Enums;

namespace ParkingHereApi.Seeder
{
    public class ParkingHereSeeder
    {
        private readonly ParkingDbContext _dbContext;

        public ParkingHereSeeder(ParkingDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                var pendingMigrations = _dbContext.Database.GetPendingMigrations();
                if (pendingMigrations != null && pendingMigrations.Any())
                {
                    _dbContext.Database.Migrate();
                }

                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }

                if (!_dbContext.Parkings.Any())
                {
                    var parkings = GetParkings();
                    _dbContext.Parkings.AddRange(parkings);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "Admin"
                },
                new Role()
                {
                    Name = "User"
                },
                new Role()
                {
                    Name = "Tenant"
                },
                new Role()
                {
                    Name = "Owner"
                }
            };

            return roles;
        }

        private IEnumerable<Parking> GetParkings()
        {
            var parkings = new List<Parking>()
            {
                new Parking()
                {
                    Name = "Parking przy lotnisku",
                    Description = "You can park here!",
                    Type = "",
                    ContactEmail = "contact@parking-przy-lotnisku.com",
                    ContactNumber = "456789345",
                    Spots = new List<ParkingSpot>()
                    {
                        new ParkingSpot()
                        {
                            Type = ParkingSpotType.Standard.ToString(),
                            Price = 59.90M,
                            IsAvailable = false,
                            StartDate = new DateTime(2023, 1, 12),
                            EndDate = new DateTime(2023, 1, 16),
                        },
                        new ParkingSpot()
                        {
                            Type = ParkingSpotType.Standard.ToString(),
                            Price = 59.90M,
                            IsAvailable = false,
                            StartDate = new DateTime(2023, 1, 11),
                            EndDate = new DateTime(2023, 1, 13),
                        },
                        new ParkingSpot()
                        {
                            Type = ParkingSpotType.Standard.ToString(),
                            Price = 59.90M,
                            IsAvailable = true,
                        },
                        new ParkingSpot()
                        {
                            Type = ParkingSpotType.Standard.ToString(),
                            Price = 59.90M,
                            IsAvailable = true,
                        },
                        new ParkingSpot()
                        {
                            Type = ParkingSpotType.Vip.ToString(),
                            Price = 99.90M,
                            IsAvailable = false,
                            StartDate = new DateTime(2023, 1, 19),
                            EndDate = new DateTime(2023, 1, 23),
                        },
                        new ParkingSpot()
                        {
                            Type = ParkingSpotType.Bus.ToString(),
                            Price = 79.90M,
                            IsAvailable = false,
                            StartDate = new DateTime(2023, 1, 13),
                            EndDate = new DateTime(2023, 1, 14),
                        },
                        new ParkingSpot()
                        {
                            Type = ParkingSpotType.Bus.ToString(),
                            Price = 79.90M,
                            IsAvailable = true,
                        },
                    },
                    Address = new Address()
                    {
                        City = "Warszawa",
                        Street = "Warszawska 1",
                        PostalCode = "01-111"
                    }
                },
                new Parking()
                {
                    Name = "Parking przy zamku",
                    Description = "Don't even think about parking elsewhere.",
                    Type = "",
                    ContactEmail = "contact@parking-przy-zamku.com",
                    ContactNumber = "123123123",
                    Spots = new List<ParkingSpot>()
                    {
                        new ParkingSpot()
                        {
                            Type = ParkingSpotType.Standard.ToString(),
                            Price = 49.90M,
                            IsAvailable = false,
                            StartDate = new DateTime(2023, 2, 13),
                            EndDate = new DateTime(2023, 2, 14),
                        },
                        new ParkingSpot()
                        {
                            Type = ParkingSpotType.Standard.ToString(),
                            Price = 49.90M,
                            IsAvailable = false,
                            StartDate = new DateTime(2023, 1, 13),
                            EndDate = new DateTime(2023, 1, 14),
                        },
                        new ParkingSpot()
                        {
                            Type = ParkingSpotType.Standard.ToString(),
                            Price = 49.90M,
                            IsAvailable = true,
                        },
                        new ParkingSpot()
                        {
                            Type = ParkingSpotType.Standard.ToString(),
                            Price = 49.90M,
                            IsAvailable = true,
                        },
                        new ParkingSpot()
                        {
                            Type = ParkingSpotType.Vip.ToString(),
                            Price = 89.90M,
                            IsAvailable = false,
                            StartDate = new DateTime(2023, 2, 23),
                            EndDate = new DateTime(2023, 2, 24),
                        },
                        new ParkingSpot()
                        {
                            Type = ParkingSpotType.Bus.ToString(),
                            Price = 69.90M,
                            IsAvailable = false,
                            StartDate = new DateTime(2023, 2, 13),
                            EndDate = new DateTime(2023, 2, 14),
                        },
                        new ParkingSpot()
                        {
                            Type = ParkingSpotType.Bus.ToString(),
                            Price = 69.90M,
                            IsAvailable = true,
                        },
                    },
                    Address = new Address()
                    {
                        City = "Gdańsk",
                        Street = "Gdańska 2",
                        PostalCode = "02-222"
                    } 
                }
            };

            return parkings;
        }
    }
}