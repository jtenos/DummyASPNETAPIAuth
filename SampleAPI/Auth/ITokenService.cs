namespace SampleAPI.Auth;

public interface ITokenService
{
    string BuildToken(IConfiguration config, string userName, int orgID);
}
