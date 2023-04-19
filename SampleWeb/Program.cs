WebApplication app = WebApplication.CreateBuilder(args).Build();
app.UseFileServer();
app.Run();
