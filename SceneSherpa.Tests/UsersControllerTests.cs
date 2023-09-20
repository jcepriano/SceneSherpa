using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using SceneSherpa.DataAccess;
using SceneSherpa.Models;

namespace SceneSherpa.Tests
{
    [Collection("Async")]
    public class UsersControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public UsersControllerTests(WebApplicationFactory<Program> factory)
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
        public async Task New_ReturnsForm()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/users/new");
            var html = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
            Assert.Contains("<form method=\"post\" action=\"/users\">", html);
            Assert.Contains("<button type=\"submit\" class=\"submit-button\">Create Account</button>", html);
        }

        [Fact]
        public async Task IndexPost_CreatesNewUser()
        {
            var client = _factory.CreateClient();
            User user = new()
            {
                Name = "John Doe",
                Username = "jdoe123",
                Email = "jdoe123@gmail.com",
                Password = "password123",
                Age = 20
            };

            var FormData = new Dictionary<string, string>
            {
                {"Name", "John Doe" },
                {"Username", "jdoe123" },
                {"Email", "jdoe123@gmail.com" },
                {"Password", "password123" },
                {"Age", "20" }
            };

            user.Email = user.ReturnEncryptedString(user.Email);
            user.Name = user.ReturnEncryptedString(user.Name);

            var response = await client.PostAsync("/users", new FormUrlEncodedContent(FormData));
            var html = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            Assert.Contains(user.Email, html);
            Assert.Contains(user.Name, html);
        }
    }
}