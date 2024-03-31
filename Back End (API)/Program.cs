using Z0key.Services.DatabaseServices;

var builder = WebApplication.CreateBuilder(args);
{
    
    builder.Services.AddControllers();
    builder.Services.AddScoped<IDatabaseService, DatabaseService>();

}

// Add services to the container.




var app = builder.Build();
{
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();

    app.MapControllers();

    app.Run();

}






