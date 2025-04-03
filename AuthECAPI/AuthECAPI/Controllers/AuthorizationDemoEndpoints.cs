using Microsoft.AspNetCore.Authorization;

namespace AuthECAPI.Controllers
{
    public static class AuthorizationDemoEndpoints
    {
        public static IEndpointRouteBuilder MapAuthorizationDemoEndpoints(this IEndpointRouteBuilder app)
        {

            app.MapGet("/AdminOnly", AdminOnly);
            //Instead of a method lets make use of Lambda expression
            //Authorize attribute added before lambda as it is the function
            app.MapGet("/AdminOrTeacherOnly", [Authorize(Roles = "Admin,Teacher")] () =>
            {
                return "Admin or Teacher";
            });

            app.MapGet("/LibraryMembersOnly", [Authorize(Policy = "HasLibraryID")] () =>
            {
                return "Library Members Only";
            });

            app.MapGet("/ApplyForMaternityLeave", [Authorize(Roles ="Teacher", Policy = "FemalesOnly")] () =>
            {
                return "Apply for Maternity Leave";
            });

            app.MapGet("/Under10AndFemale", 
                [Authorize(Policy = "Under10")] 
                [Authorize(Policy = "FemalesOnly")] 
                () =>
                {
                    return "Females Under 10";
                });

            return app;
        }

        //Middleware will identify whether the user as Admin role 
        [Authorize(Roles ="Admin")]
        private static string AdminOnly()
        {
            return "Admin Only";
        }
    }
}
