using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SMSHub.APIs.Extensions;
using SMSHub.Core.Entities.Identity;
using SMSHub.Repository.Identity;
using StackExchange.Redis;
using SMSHub.Repository.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region configure services (Add services to the container)
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<SMSHubContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));//allow DI for dbcontext
});
builder.Services.AddSingleton<IConnectionMultiplexer>(Options =>
{
    var Connection = builder.Configuration.GetConnectionString("RedisConnection");
    return ConnectionMultiplexer.Connect(Connection);
});

builder.Services.AddApplicationServices();//call extension method that has services
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddDbContext<AppIdentityDbContext>(Options =>
{
    Options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
});
#endregion

var app = builder.Build();

#region Update-Database (instead of npm)

using var scope = app.Services.CreateScope();//container for services
var services = scope.ServiceProvider;//actual services
var LoggerFactory = services.GetRequiredService<ILoggerFactory>();
try
{
    var dbcontext = services.GetRequiredService<SMSHubContext>();
    await dbcontext.Database.MigrateAsync();
   
    var Identitydbcontext = services.GetRequiredService<AppIdentityDbContext>();
    await Identitydbcontext.Database.MigrateAsync();
    //data seeding
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await AppIdentityDbContextSeed.SeedUserAsync(userManager, roleManager);

    // await StoreContextSeed.SeedAsync(dbcontext);
}
catch (Exception ex)
{
    var logger = LoggerFactory.CreateLogger<Program>();
    logger.LogError(ex, "an error occured during applying the migration");
}

#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseSwaggerMiddlewares();
}
app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseHttpsRedirection();
app.UseStaticFiles();//load images
app.UseCors("AllowMyFrontend");
app.UseAuthentication();

app.UseAuthorization();


app.MapControllers();

app.Run();


//cleaning up program class
/*
 * 
 * 
 * 
 * 
 */