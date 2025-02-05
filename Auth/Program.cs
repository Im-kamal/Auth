using Auth.Data.Identity;
using Auth.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static DashboardController;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppIdentityDbContext>(Options =>
{
    Options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
});

builder.Services.AddScoped<ITokenServices, TokenServices>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppIdentityDbContext>();
builder.Services.AddTransient<IEmailService, SendGridEmailService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using var Scope = app.Services.CreateScope();

var Services = Scope.ServiceProvider;

var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();
try
{
    var IdentityContext = Services.GetRequiredService<AppIdentityDbContext>();

    await IdentityContext.Database.MigrateAsync();

    var UserManager = Services.GetRequiredService<UserManager<IdentityUser>>();
    await AppIdentityDbContextSeed.SeedUserAsync(UserManager);

}
catch (Exception ex)
{
    var Logger = LoggerFactory.CreateLogger<Program>();
    Logger.LogError(ex, "An Error During Applying Migration");
}

app.Run();
