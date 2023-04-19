using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleAPI.Auth;
using System.Security.Claims;

namespace SampleAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AdminController : ControllerBase
{
    public record class AdminResult(bool Success, string Message);

    [HttpGet]
    [Authorize(Policy = Policies.Admin)]
    public AdminResult Get()
    {
        ClaimsIdentity user = (ClaimsIdentity)User.Identity!;
        int orgID = int.Parse(user.Claims.First(x => x.Type == CustomClaimTypes.OrgID).Value);

        string name = user.Claims.First(x => x.Type == ClaimTypes.Name).Value;

        return new(true, $"You are an admin! Your name is {name} and your org is {orgID}");
    }
}
