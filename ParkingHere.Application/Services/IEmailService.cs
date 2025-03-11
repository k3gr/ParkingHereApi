using ParkingHereApi.Common.Models;

namespace ParkingHereApi.Services
{
    public interface IEmailService
    {
        bool SendMail(CreateEmail createEmail);
    }
}
