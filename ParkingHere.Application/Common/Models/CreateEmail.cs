namespace ParkingHereApi.Common.Models
{
    public class CreateEmail
    {
        public string EmailTo { get; set; }
        public string EmailToName { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
    }
}
