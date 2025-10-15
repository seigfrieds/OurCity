using Microsoft.AspNetCore.Authorization;

namespace OurCity.Api.Services.Authorization.CanCreatePosts;

public class CanCreatePostsHandler : AuthorizationHandler<CanCreatePostsRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CanCreatePostsRequirement requirement
    )
    {
        var isUserAuthenticated = context.User.Identity?.IsAuthenticated ?? false;
        
        if (isUserAuthenticated)
            context.Succeed(requirement);
        
        return Task.CompletedTask;
    }
}
