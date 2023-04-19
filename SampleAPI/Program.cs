using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SampleAPI;
using SampleAPI.Auth;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<TokenService>();
builder.Services.AddCors();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.Admin, policy =>
    {
        policy.RequireClaim(CustomClaimTypes.Admin);
    });
    options.AddPolicy(Policies.HumanResources, policy =>
    {
        policy.RequireClaim(CustomClaimTypes.HumanResources);
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    (string issuer, string audience, byte[] key) = builder.Configuration.Jwt();
    opt.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(key)
        //ClockSkew is 5 minutes by default, so this still passes for up to 5 minutes
    };
});

builder.Services.AddControllers();

WebApplication app = builder.Build();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
