using AuthECAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Reflection;

namespace AuthECAPI.Tests
{
    public class IdentityUserEndpointsTests
    {
        [Fact]
        public async Task FetchBooks_ReturnsSeededBooks()
        {
            // Arrange - in-memory EF Core
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "FetchBooks_TestDb")
                .Options;

            using var context = new AppDbContext(options);
            context.Books.AddRange(
                new Book { BookTitle = "Book1", Genre = "Tech", IsBorrowed = false },
                new Book { BookTitle = "Book2", Genre = "History", IsBorrowed = true, BorrowedByEmail = "user@example.com" }
            );
            await context.SaveChangesAsync();

            // Use reflection to call private static handler
            var endpointsType = typeof(AuthECAPI.Controllers.IdentityUserEndpoints);
            var method = endpointsType.GetMethod("FetchBooks", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(method);

            // Invoke and await Task<IResult> (dynamic makes awaiting the generic Task<T> simple in tests)
            var invocation = method.Invoke(null, new object[] { context });
            Assert.NotNull(invocation); // Ensure invocation is not null to avoid CS8600
            var result = await (dynamic)invocation!;
            Assert.NotNull(result);

            // Read Value property (OkObjectResult produced by Results.Ok(object) exposes Value)
            var valueProp = result.GetType().GetProperty("Value");
            Assert.NotNull(valueProp);
            var value = valueProp.GetValue(result);
            Assert.NotNull(value);

            var list = ((IEnumerable)value).Cast<object>().ToList();
            Assert.Equal(2, list.Count);

            var first = list[0];
            var titleProp = first.GetType().GetProperty("BookTitle");
            Assert.Equal("Book1", titleProp.GetValue(first));
        }
    }
}