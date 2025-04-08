using AuthECAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthECAPI.Controllers
{
    public class UserRegistrationModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public int? LibraryID { get; set; }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class PasswordModel
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
    public static class IdentityUserEndpoints
    {
        public static IEndpointRouteBuilder MapIdentityUserEndpoints(this IEndpointRouteBuilder app)
        {
            //Since we MapGroup we do not need /Api
            //app.MapPost("/api/signup", CreateUser);
            //app.MapPost("/signup", CreateUser).AllowAnonymous();//Can be done with this as well, but we will use [AllowAnonymous]
            app.MapPost("/signup", CreateUser);
            //app.MapPost("api/signin", SignIn);
            app.MapPost("/signin", SignIn);

            app.MapPost("/forgotpasswordwithemail", ForgotPassword);

            return app;
        }

        [AllowAnonymous]
        private static async Task<IResult> CreateUser(UserManager<AppUser> userManager,
                [FromBody] UserRegistrationModel userRegistrationModel)
        {
            AppUser user = new AppUser()
            {
                Email = userRegistrationModel.Email,
                FullName = userRegistrationModel.FullName,
                UserName = userRegistrationModel.Email,
                Gender = userRegistrationModel.Gender,
                DOB = DateOnly.FromDateTime(DateTime.Now.AddYears(-userRegistrationModel.Age)).ToString(),
                LibraryID = userRegistrationModel.LibraryID,
            };

            var result = await userManager.CreateAsync(user, userRegistrationModel.Password);
            await userManager.AddToRoleAsync(user, userRegistrationModel.Role);
            if (result.Succeeded)
                return Results.Ok(result);
            else
                return Results.BadRequest(result);

        }
        [AllowAnonymous]
        private static async Task<IResult> SignIn(UserManager<AppUser> userManager,
                [FromBody] LoginModel loginModel,
                IOptions<AppSettings> appSettings)
        {
            var user = await userManager.FindByEmailAsync(loginModel.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                var roles = await userManager.GetRolesAsync(user);
                var signInKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    appSettings.Value.JWTSecret));

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(new Claim[]
                {
                    new Claim("userID",user.Id.ToString()),
                    new Claim("gender", user.Gender.ToString()),
                    //new Claim( ("Age",DateOnly.Parse((DateTime.Now.Year - user.DOB.Year).ToString())),
                    //new Claim("Age",DateOnly.FromDateTime(DateTime.Now.AddYears(-user.Age))),
                    //new Claim(ClaimTypes.Role,roles.First())
                    new Claim("age",(DateTime.Now.Year - DateTime.Parse(user.DOB).Year).ToString()),
                    new Claim(ClaimTypes.Role,roles.First()),

                });
                if(user.LibraryID != null)
                {
                    claimsIdentity.AddClaim(new Claim("libraryID", user.LibraryID.ToString()!));
                }
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claimsIdentity,
                    Expires = DateTime.Now.AddMinutes(1),
                    SigningCredentials = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Results.Ok(new { token });
            }
            else
            {
                return Results.BadRequest(new { message = "Username or password is incorrect." });
            }
        }

        [AllowAnonymous]

        private static async Task<IResult> ForgotPassword(UserManager<AppUser> userManager,
                [FromBody] PasswordModel passwordModel)
        {
            var user = await userManager.FindByEmailAsync(passwordModel.Email);
            if(user != null)
            {
                var result = await userManager.ChangePasswordAsync(user,passwordModel.OldPassword,passwordModel.NewPassword);
                if (result.Succeeded)
                {
                    return Results.Ok(result);
                }
                else
                {
                    return Results.BadRequest(result);
                }
            }
            else
            {
                return Results.BadRequest(new {message = "User name incorrect"});
            }
            //return Results.Ok(new { message = "Password changed " });
        }

    }
}
