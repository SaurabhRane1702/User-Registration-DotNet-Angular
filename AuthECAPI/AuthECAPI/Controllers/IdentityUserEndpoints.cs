using AuthECAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Data;
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

    public class BookModel {
        public int Id { get; set; }
        public string BookTitle { get; set; }
        public string Genre { get; set; }
        public bool IsBorrowed { get; set; }
        public string? BorrowedByEmail { get; set; }
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
            app.MapPost("/addbooks", AddBooks);
            app.MapGet("/fetchtimetable", FetchTimeTable);
            app.MapGet("/fetchbooks", FetchBooks);
            app.MapGet("/fetchborrowedbooks", FetchBorrowedBooks);
            app.MapPost("/submitbooks", SubmitBooks);
            app.MapGet("/fetchallusers", FetchAllUsers);
            app.MapGet("/fetchallemail", FetchUserEmail);
            app.MapGet("/fetchuseronemail", FetchUserOnEmail);
            app.MapPost("/borrowbook", BorrowBook);
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
                    Expires = DateTime.Now.AddMinutes(60),
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
            if (timeTables == null)
            {
                return Results.BadRequest(new { message = "No time table found for this user." });
            }
            else if (timeTables.Count == 0)
            {
                return Results.NoContent();
            }
            else
            {
                return Results.Ok(timeTables);
            }
        }

        [Authorize(Roles = "Admin,Student")]
        private static async Task<IResult> FetchBooks([FromServices] AppDbContext context)
        {
            // Fetch all books with their borrowed status and borrower details  
            var books = await context.Books
                                     .Select(b => new
                                     {
                                         b.Id,
                                         b.BookTitle,
                                         b.Genre,
                                         b.IsBorrowed,
                                         BorrowedByEmail = b.IsBorrowed ? b.BorrowedByEmail : null // Include borrower email if borrowed  
                                     })
                                     .ToListAsync();

            // Check if any books exist  
            if (books == null || books.Count == 0)
            {
                return Results.NoContent(); // Return 204 No Content if no books are found  
            }

            return Results.Ok(books); // Return the list of books  
        }

        [Authorize(Roles ="Student")]
        private static async Task<IResult> FetchBorrowedBooks([FromServices] AppDbContext context,UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == "userID")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Results.BadRequest(new { message = "Invalid user claims." });
            }

            var appUser = await userManager.FindByIdAsync(userIdClaim);
            if (appUser == null)
            {
                return Results.BadRequest(new { message = "User not found." });
            }
            var email = appUser.Email;
            //Fetch books from book table where borrowedbyemail = email
            var borrowedBooks = await context.Books
                .Where(b => b.BorrowedByEmail == email)
                .Select(b => new BookModel
                {
                    Id = b.Id,
                    BookTitle =  b.BookTitle,
                    Genre =  b.Genre,
                    IsBorrowed = b.IsBorrowed,
                    BorrowedByEmail = b.BorrowedByEmail
                })
                .ToListAsync();
            if (borrowedBooks == null || borrowedBooks.Count == 0)
            {
                return Results.NoContent();
            }
            else
            {
                return Results.Ok(borrowedBooks);
            }
        }

        [Authorize(Roles ="Admin,Student")]
        private static async Task<IResult> SubmitBooks([FromServices] AppDbContext context, [FromQuery] int bookId)
        {
            var book = await context.Books.FindAsync(bookId);
            if (book == null)
            {
                return Results.NotFound(new { message = "Book not found." });
            }
            // Update book record to mark as returned
            book.IsBorrowed = false;
            book.BorrowedByEmail = null;

            try
            {
                context.Books.Update(book);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Results.Conflict(new { message = "Concurrency conflict occurred while submitting the book. Please try again." });
            }
            return Results.Ok(new
            {
                message = "Book submitted successfully.",
            });
        }

        [Authorize(Roles ="Admin")]
        private static async Task<IResult> AddBooks([FromServices] AppDbContext context, [FromBody] BookModel bookModel)
        {
            var b = new Book
            {
                BookTitle = bookModel.BookTitle,
                Genre = bookModel.Genre,
                IsBorrowed = bookModel.IsBorrowed,
                BorrowedByEmail = bookModel.IsBorrowed ? bookModel.BorrowedByEmail : null
            };
            context.Books.Add(b);
            await context.SaveChangesAsync();

            return Results.Ok(b);


        }

        [Authorize(Roles = "Admin")]
        private static async Task<IResult> FetchAllUsers([FromServices] AppDbContext context, UserManager<AppUser> userManager)
        {
            //return Results.Ok(await userManager.Users.ToListAsync());
            var users = await userManager.Users.ToListAsync();
            var userDtos = new List<UserRegistrationModel>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                userDtos.Add(new UserRegistrationModel()
                {
                    Email = user.Email ?? string.Empty, // Fix CS8601: Ensure Email is not null
                    FullName = user.FullName,
                    Age = DateTime.Now.Year - DateTime.Parse(user.DOB).Year,
                    Gender = user.Gender,
                    LibraryID = user.LibraryID,
                    Role = roles.FirstOrDefault() ?? string.Empty // Fix CS8601: Ensure Role is not null
                });
            }
            return Results.Ok(userDtos);
        }

        [Authorize(Roles ="Admin")]
        private static async Task<IResult> FetchUserOnEmail([FromQuery] string email, [FromServices] AppDbContext context, UserManager<AppUser> userManager)
        {
            var user = await userManager.FindByEmailAsync(email);
            if(user == null)
            {
                return Results.NotFound(new { message = "No User with this EmailID" });
            }
            var role = await userManager.GetRolesAsync(user);
            if (user != null)
            {
                return Results.Ok(new UserRegistrationModel()
                {
                    Email = user.Email ?? string.Empty, // Fix CS8601: Ensure Email is not null
                    FullName = user.FullName,
                    Age = DateTime.Now.Year - DateTime.Parse(user.DOB).Year,
                    Gender = user.Gender,
                    LibraryID = user.LibraryID,
                    Role = role.FirstOrDefault() ?? string.Empty // Fix CS8601: Ensure Role is not null

                });
            }
            return Results.NotFound(new { message = "User not found." }); // Add return for missing user
        }

        [Authorize(Roles = "Admin")]
        private static async Task<IResult> FetchUserEmail(UserManager<AppUser> userManager)
        {
            var appUsers = await userManager.Users.ToListAsync(); // Fetch all users as a list
            var emails = appUsers.Select(user => user.Email).ToList(); // Extract emails into a list
            return Results.Ok(emails); // Return the list of emails
        }

        [Authorize(Roles ="Student,Teacher")]
        private static async Task<IResult> BorrowBook([FromQuery] int bookId, ClaimsPrincipal user, [FromServices] AppDbContext context, UserManager<AppUser> userManager)
        {
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == "userID")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Results.BadRequest(new { message = "Invalid user claims." });
            }

            var appUser = await userManager.FindByIdAsync(userIdClaim);
            if (appUser == null)
            {
                return Results.BadRequest(new { message = "User not found." });
            }

            var book = await context.Books.FindAsync(bookId);
            if (book == null)
            {
                return Results.NotFound(new { message = "Book not found." });
            }

            // 3. Check borrow state
            if (book.IsBorrowed)
            {
                return Results.BadRequest(new { message = "Book is already borrowed." });
            }

            // 4. Update book record to mark as borrowed by this user
            book.IsBorrowed = true;
            book.BorrowedByEmail = appUser.Email;

            try
            {
                context.Books.Update(book);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Results.Conflict(new { message = "Concurrency conflict occurred while borrowing the book. Please try again." });
            }

            return Results.Ok(new
            {
                message = "Book borrowed successfully.",
                book.Id,
                book.BookTitle,
                book.Genre,
                book.IsBorrowed,
                book.BorrowedByEmail
            });

        }


    }
}
