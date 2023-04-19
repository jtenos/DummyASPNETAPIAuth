using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleAPI.Auth;
using System.Security.Claims;
using System.Text.Json;

namespace SampleAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class HumanResourcesController : ControllerBase
{
    public record class HumanResourcesResult(bool Success, string Message);

    [HttpGet]
    [Authorize(Policy = Policies.HumanResources)]
    public HumanResourcesResult Get()
    {
        ClaimsIdentity user = (ClaimsIdentity)User.Identity!;
        int orgID = int.Parse(user.Claims.First(x => x.Type == CustomClaimTypes.OrgID).Value);

        string name = user.Claims.First(x => x.Type == ClaimTypes.Name).Value;

        return new(true, $"You are in HR! Your name is {name} and your org is {orgID}");
    }
}
