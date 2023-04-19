using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleAPI.Auth;

namespace SampleAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _config;

    public LoginController(ITokenService tokenService, IConfiguration config)
    {
        _tokenService = tokenService;
        _config = config;
    }

    public record class LoginRequest(string UserName, int OrgID);
    public record class LoginResult(bool Success, string? Jwt);

    [HttpPost]
    [AllowAnonymous]
    public LoginResult Post(LoginRequest req)
    {
        if (req.UserName == "bad") { return new(false, null); }

        // Validate user's name and password and pull role info from database
        string jwt = _tokenService.BuildToken(_config, req.UserName, req.OrgID);
        return new(true, jwt);
    }
}
