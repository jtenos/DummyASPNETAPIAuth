namespace SampleAPI;

public static class ConfigExtensions
{
    public static (
        string Issuer,
        string Audience,
        byte[] Key
        ) Jwt(this IConfiguration config) => (
            Issuer: config["Jwt:Issuer"]!,
            Audience: config["Jwt:Audience"]!,
            Key: Convert.FromHexString(config["Jwt:Key"]!)
        );
}
