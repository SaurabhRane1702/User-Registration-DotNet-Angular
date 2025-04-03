using AuthECAPI.Controllers;
using AuthECAPI.Extensions;
using AuthECAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//Moved to extension method
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();


// Services from Identity Core --> Moved to Extensions Methods
//Below commented code is when we are written void from the extension method 
//but if we return IServiceCollection we can achive the next line of uncommented code.
//builder.Services.AddIndetityHandlerAndStores();
//builder.Services.ConfigureIdentityOptions();

builder.Services.AddSwaggerExplorer()
                .InjectDBContext(builder.Configuration)
                //Adding dependency injection
                .AddAppConfig(builder.Configuration)
                .AddIndetityHandlerAndStores()
                .ConfigureIdentityOptions()
                .AddIndentityAuth(builder.Configuration);



//builder.Services.AddDbContext<AppDbContext>(options => 
//        options.UseSqlServer(builder.Configuration.GetConnectionString("DevDB"))); 

//This is again chained above 
//builder.Services.InjectDBContext(builder.Configuration);

//Adding JWT Authentication 
//moved to extension method


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.ConfigureSwaggerExplorer();

#region Configure. CORS
app.ConfigureCORS(builder.Configuration);
#endregion
app.AddIdentityAuthMiddleare();

app.MapControllers();

app.MapGroup("/api")
   .MapIdentityApi<AppUser>();
app.MapGroup("/api")
    .MapAccountEndpoints()
    .MapIdentityUserEndpoints()
    .MapAuthorizationDemoEndpoints();

app.Run();
