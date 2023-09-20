using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using SceneSherpa.Controllers;
using SceneSherpa.DataAccess;

namespace SceneSherpa.Tests
{
    [Collection("Async")]
    public class ReviewsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ReviewsControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        private SceneSherpaContext GetDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<SceneSherpaContext>();
            optionsBuilder.UseInMemoryDatabase("TestDatabase");

            var context = new SceneSherpaContext(optionsBuilder.Options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }

        [Fact]
        public void Test1()
        {

        }
    }
}