using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace OurCity.Api.Services.Authorization;

public interface IPolicyService
{
    Task<bool> CheckPolicy(ClaimsPrincipal user, Policy policy);
}

public class PolicyService : IPolicyService
{
    private readonly IAuthorizationService _authorizationService;

    public PolicyService(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    public async Task<bool> CheckPolicy(ClaimsPrincipal user, Policy policy)
    {
        var isAuthorized = await _authorizationService.AuthorizeAsync(user, policy);

        return isAuthorized.Succeeded;
    }
}
