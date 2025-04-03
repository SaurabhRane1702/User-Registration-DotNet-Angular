using AuthECAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthECAPI.Extensions
{
    public static class IndentityExtensions
    {
        public static IServiceCollection AddIndetityHandlerAndStores(this IServiceCollection services)
        {

            services.AddIdentityApiEndpoints<AppUser>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>();

            return services;
        }

        public static IServiceCollection ConfigureIdentityOptions(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(
                options =>
                {  
                 options.Password.RequireDigit = false;
                 options.Password.RequireUppercase = false;
                 options.Password.RequireLowercase = false;
                 options.User.RequireUniqueEmail = true;
                }
            );

            return services;
        }

        //Auth = Authentication
        public static IServiceCollection AddIndentityAuth(this IServiceCollection services,
            IConfiguration config)
            {
                services.AddAuthentication(
                    //x =>
                //{
                    //Below we speicify which authentication scheme we want, we can have multiple scheme in single application if required
                    //x.DefaultAuthenticateScheme =
                    //x.DefaultChallengeScheme =
                    //x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    JwtBearerDefaults.AuthenticationScheme
                //}
                ).AddJwtBearer(y =>
                {
                    y.SaveToken = false;
                    y.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(
                                config["AppSettings:JWTSecret"]!)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

                services.AddAuthorization(options =>
                {
                    options.FallbackPolicy = new AuthorizationPolicyBuilder()
                        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                        .RequireAuthenticatedUser()
                        .Build();

                    options.AddPolicy("HasLibraryID", policy => policy.RequireClaim("libraryID"));
                    options.AddPolicy("FemalesOnly", policy => policy.RequireClaim("gender", "Female"));
                    options.AddPolicy("Under10", policy => policy.RequireAssertion(context =>
                        Int32.Parse(context.User.Claims.First(x => x.Type == "age").Value) < 10));
                });
                return services;
        }

        public static WebApplication AddIdentityAuthMiddleare(this WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
