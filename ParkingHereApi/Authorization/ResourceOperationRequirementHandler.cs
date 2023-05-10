using Microsoft.AspNetCore.Authorization;
using ParkingHereApi.Entities;
using System.Security.Claims;

namespace ParkingHereApi.Authorization
{
    public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, Parking>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, Parking parking)
        {
            if (requirement.ResourceOperation == Enums.ResourceOperation.Read ||
                requirement.ResourceOperation == Enums.ResourceOperation.Create)
            {
                context.Succeed(requirement);
            }

            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (parking.CreatedById == int.Parse(userId))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
