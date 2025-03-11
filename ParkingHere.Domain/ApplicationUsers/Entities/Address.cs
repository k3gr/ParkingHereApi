using ParkingHere.Domain.Parkings.Entities;

namespace ParkingHere.Domain.ApplicationUsers.Entities
{
    public class Address
    {
        public Address()
        {
        }

        public Address(string city, string street, string postalCode)
        {
            City = city;
            Street = street;
            PostalCode = postalCode;
        }

        public Guid Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }

        public virtual Parking Parking { get; set; }
    }
}
