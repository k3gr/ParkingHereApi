namespace ParkingHere.Application.ApplicationUsers.DTO;
public class JwtDto
{
    public string AccessToken { get; set; }
    public DateTime Expires { get; set; }
}
