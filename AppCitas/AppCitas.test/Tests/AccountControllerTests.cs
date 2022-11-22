using AppCitas.Service.DTOs;
using AppCitas.Service.Entities.DOTs;
using AppCitas.test.Helpers;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace AppCitas.test.Test
{
    public class AccountControllerTests
    {
        private string apiRoute = "api/account";
        private readonly HttpClient _client;
        private HttpResponseMessage httpResponse;
        private string requestUri;
        private string registeredObject;
        private HttpContent httpContent;
        public AccountControllerTests()
        {
            _client = TestHelper.Instance.Client;
        }

        [Theory]
        [InlineData("BadRequest", "lisa", "KnownAs", "Gender", "2000-01-01", "City", "Country", "Password")]
        public async Task Register_Fail(string statusCode, string username, string knownAs, string gender, DateTime birthday, string city, string country, string password)
        {
            // Arrange
            requestUri = $"{apiRoute}/register";
            var registerDto = new RegisterDto
            {
                Username = username,
                KnownAs = knownAs,
                Gender = gender,
                Birthday = birthday,
                City = city,
                Country = country,
                Password = password
            };
            registeredObject = GetRegisterObject(registerDto);
            httpContent = GetHttpContent(registeredObject);

            // Act
            httpResponse = await _client.PostAsync(requestUri, httpContent);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        //cambiar el nombre arturo

        [Theory]
        [InlineData("OK", "arturo", "KnownAs", "Gender", "2000-01-01", "City", "Country", "Password")]
        public async Task Register_ReturnOk(string statusCode, string username, string knownAs, string gender, DateTime birthday, string city, string country, string password)
        {
            // Arrange
            requestUri = $"{apiRoute}/register";
            var registerDto = new RegisterDto
            {
                Username = username,
                KnownAs = knownAs,
                Gender = gender,
                Birthday = birthday,
                City = city,
                Country = country,
                Password = password
            };
            registeredObject = GetRegisterObject(registerDto);
            httpContent = GetHttpContent(registeredObject);

            // Act
            httpResponse = await _client.PostAsync(requestUri, httpContent);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("Unauthorized", "lisa", "notapassword")]
        public async Task Login_Fail(string statusCode, string username, string password)
        {
            // Arrange
            requestUri = $"{apiRoute}/login";
            var loginDto = new LoginDTo
            {
                Username = username,
                Password = password
            };
            registeredObject = GetRegisterObject(loginDto);
            httpContent = GetHttpContent(registeredObject);

            // Act
            httpResponse = await _client.PostAsync(requestUri, httpContent);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("OK", "lisa", "Password")]
        public async Task Login_ReturnOK(string statusCode, string username, string password)
        {
            // Arrange
            requestUri = $"{apiRoute}/login";
            var loginDto = new LoginDTo
            {
                Username = username,
                Password = password
            };
            registeredObject = GetRegisterObject(loginDto);
            httpContent = GetHttpContent(registeredObject);

            // Act
            httpResponse = await _client.PostAsync(requestUri, httpContent);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        #region Privated methods
        private static string GetRegisterObject(RegisterDto registerDto)
        {
            var entityObject = new JObject()
            {
                { nameof(registerDto.Username), registerDto.Username },
                { nameof(registerDto.KnownAs), registerDto.KnownAs },
                { nameof(registerDto.Gender), registerDto.Gender },
                { nameof(registerDto.Birthday), registerDto.Birthday },
                { nameof(registerDto.City), registerDto.City },
                { nameof(registerDto.Country), registerDto.Country },
                { nameof(registerDto.Password), registerDto.Password }
            };

            return entityObject.ToString();
        }

        private static string GetRegisterObject(LoginDTo loginDto)
        {
            var entityObject = new JObject()
            {
                { nameof(loginDto.Username), loginDto.Username },
                { nameof(loginDto.Password), loginDto.Password }
            };
            return entityObject.ToString();
        }

        private StringContent GetHttpContent(string objectToEncode)
        {
            return new StringContent(objectToEncode, Encoding.UTF8, "application/json");
        }

        #endregion
    }
}
