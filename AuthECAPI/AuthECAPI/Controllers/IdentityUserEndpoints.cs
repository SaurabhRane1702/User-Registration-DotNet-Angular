using AuthECAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
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

    public class TimeTableModel
    {
        public string SubjectName { get; set; }
        public string ClassName { get; set; }
        public string InputEmail { get; set; }
        public string TimeActivity { get; set; }
        public string WeekDayActivity { get; set; }
        public string TeacherSelection { get; set; }
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
            app.MapPost("/addtimetable", AddTimeTable);
            app.MapGet("/fetchtimetable", FetchTimeTable);

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
            Console.WriteLine($"User data : ", user);
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
                    Expires = DateTime.Now.AddMinutes(10),
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

        [Authorize(Roles ="Admin")]
        private static async Task<IResult> AddTimeTable(UserManager<AppUser> userManager, [FromBody] TimeTableModel timeTableModel, [FromServices] AppDbContext context)
        {
            var user = await userManager.FindByEmailAsync(timeTableModel.InputEmail);

            if (user != null)
            {
                var t = new TimeTable
                {
                    Subject = timeTableModel.SubjectName,
                    Class = timeTableModel.ClassName,
                    Day = timeTableModel.WeekDayActivity,
                    Teacher = timeTableModel.TeacherSelection,
                    Time = timeTableModel.TimeActivity,
                    Email = timeTableModel.InputEmail,
                    UserId = user.Id
                };

                context.TimeTables.Add(t);
                await context.SaveChangesAsync();
            }
            else
            {
                return Results.BadRequest(new { message = "User Email incorrect" });
            }

            return Results.Ok(user);
        }

        [Authorize(Roles = "Teacher, Student")]
        private static async Task<IResult> FetchTimeTable(ClaimsPrincipal user, [FromServices] AppDbContext context)
        {
            // Extract the userID from the claims
            string userID = user.Claims.First(x => x.Type == "userID").Value;

            // Query the TimeTable table to fetch records for the specific userID
            var timeTables = await context.TimeTables
                                          .Where(t => t.UserId == userID)
                                          .Select(t => new
                                          {
                                              t.Subject,
                                              t.Class,
                                              t.Day,
                                              t.Time,
                                              t.Teacher
                                          })
                                          .ToListAsync();

            // Check if any records were found, otherwise early return.
            if (timeTables == null || timeTables.Count == 0)
            {
                return Results.BadRequest(new { message = "No time table found for this user." });
            }
            else
            {
                return Results.Ok(timeTables);
            }
        }



    }
}
