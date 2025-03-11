using ParkingHere.Application.Abstractions;

namespace ParkingHere.Application.ApplicationUsers.Commands;
public record SignIn(string Email, string Password) : ICommand;