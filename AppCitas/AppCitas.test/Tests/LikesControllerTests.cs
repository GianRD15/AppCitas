using AppCitas.Service.DTOs;
using AppCitas.Service.Entities.DOTs;
using AppCitas.test.Helpers;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Net;


namespace AppCitas.test.Test
{
    public class LikesControllerTests
    {
        private string apiRoute = "api/likes";
        private readonly HttpClient _client;
        private HttpResponseMessage httpResponse;
        private string requestUri;
        private HttpContent? httpContent;

        public LikesControllerTests()
        {
            _client = TestHelper.Instance.Client;
        }

        [Theory]
        [InlineData("OK", "lisa", "Password", "rasmussen")]
        public async Task AddLike_OK(string statusCode, string username, string password, string userLiked)
        {
            // Arrange
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

            requestUri = $"{apiRoute}/" + userLiked;

            // Act
            httpResponse = await _client.PostAsync(requestUri, httpContent);
            _client.DefaultRequestHeaders.Authorization = null;

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("NotFound", "lisa", "Password", "UnexisentUser")]
        public async Task AddLike_NotFound(string statusCode, string username, string password, string userLiked)
        {
            // Arrange
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

            requestUri = $"{apiRoute}/" + userLiked;

            // Act
            httpResponse = await _client.PostAsync(requestUri, httpContent);
            _client.DefaultRequestHeaders.Authorization = null;

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }


        [Theory]
        [InlineData("BadRequest", "lisa", "Password", "lisa")]
        public async Task AddLike_BadRequest(string statusCode, string username, string password, string userLiked)
        {
            // Arrange
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

            requestUri = $"{apiRoute}/" + userLiked;

            // Act
            httpResponse = await _client.PostAsync(requestUri, httpContent);
            _client.DefaultRequestHeaders.Authorization = null;

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("BadRequest", "lisa", "Password", "mckay")]
        public async Task AddLike_BadRequest2(string statusCode, string username, string password, string userLiked)
        {
            // Arrange
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

            requestUri = $"{apiRoute}/" + userLiked;

            // Act
            httpResponse = await _client.PostAsync(requestUri, httpContent);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
            httpResponse = await _client.PostAsync(requestUri, httpContent);

            _client.DefaultRequestHeaders.Authorization = null;
            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("OK", "lisa", "Password", "todd", "liked")]
        [InlineData("OK", "lisa", "Password", "todd", "likedby")]
        public async Task GetUserLikes_OK(string statusCode, string username, string password, string userliked, string predicate)
        {
            // Arrange
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

            requestUri = $"{apiRoute}/" + userliked;

            httpResponse = await _client.PostAsync(requestUri, httpContent);
            _client.DefaultRequestHeaders.Authorization = null;

            var userLiked = await LoginHelper.LoginUser(userliked, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userLiked.Token);

            requestUri = $"{apiRoute}" + "?predicate="+predicate;

            // Act
            httpResponse = await _client.GetAsync(requestUri);
            _client.DefaultRequestHeaders.Authorization = null;

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }
    }
}
