namespace ParkingHere.Application.Common.Models
{
    public class EmailSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string ParkingName { get; set; }
        public string Password { get; set; }
        public string EmailApiKey { get; set; }
    }
}
