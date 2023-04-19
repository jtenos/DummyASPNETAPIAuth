using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SampleAPI.Auth;

public class TokenService
{
    private static readonly TimeSpan _expirationDuration = TimeSpan.FromHours(12);

    public string BuildToken(IConfiguration config, string userName, int orgID)
    {
        var (issuer, audience, key) = config.Jwt();
        Console.WriteLine(Convert.ToBase64String(key));
        Console.WriteLine(Convert.ToHexString(key));
        List<Claim> claims = new()
        {
            new(ClaimTypes.Name, userName),
            new(CustomClaimTypes.OrgID, orgID.ToString())
        };
        switch (userName)
        {
            case "ad":
                claims.Add(new(CustomClaimTypes.Admin, true.ToString()));
                break;
            case "hr":
                claims.Add(new(CustomClaimTypes.HumanResources, true.ToString()));
                break;
        }

        SymmetricSecurityKey securityKey = new(key);
        SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256Signature);
        JwtSecurityToken tokenDescriptor = new(issuer, audience, claims,
            expires: DateTime.Now.Add(_expirationDuration), signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}
