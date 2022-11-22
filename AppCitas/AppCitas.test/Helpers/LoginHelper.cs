using AppCitas.Service.Entities.DOTs;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.Json;

namespace AppCitas.test.Helpers
{
    public class LoginHelper
    {
        private static string registeredObject;
        private static StringContent httpContent;

        public static async Task<UserDto> LoginUser(string username, string password)
        {
            string requestUri = $"api/account/login";
            HttpClient client = TestHelper.Instance.Client;
            var loginDto = new LoginDTo
            {
                Username = username,
                Password = password
            };
            registeredObject = GetLoginObject(loginDto);
            httpContent = GetHttpContent(registeredObject);
            var result = await client.PostAsync(requestUri, httpContent);
            var userJson = await result.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<UserDto>(userJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return user;
        }
        #region Privated methods
        private static string GetLoginObject(LoginDTo loginDto)
        {
            var entityObject = new JObject()
            {
                { nameof(loginDto.Username), loginDto.Username },
                { nameof(loginDto.Password), loginDto.Password }
            };
            return entityObject.ToString();
        }
        public static StringContent GetHttpContent(string objectToEncode)
        {
            return new StringContent(objectToEncode, Encoding.UTF8, "application/json");
        }

        #endregion
    }
}
