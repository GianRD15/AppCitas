﻿using AppCitas.Service.Entities.DOTs;
using AppCitas.test.Helpers;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Windows.Storage;

namespace AppCitas.test.Test
{
    public class UsersControllerTests
    {
        private string apiRoute = "api/users";
        private readonly HttpClient _client;
        private HttpResponseMessage httpResponse;
        private string requestUri;
        private string registeredObject;
        private HttpContent httpContent;

        public UsersControllerTests()
        {
            _client = TestHelper.Instance.Client;
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        [Theory]
        [InlineData("OK", "lisa", "Password", "foto3.png")]
        public async Task DeletePhoto_OK(string statusCode, string username, string password, string file)
        {
            // Arrange
            MultipartFormDataContent form = new MultipartFormDataContent();
            HttpContent content = new StringContent(file);
            form.Add(content, file);

            string root = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;
            root = root.Substring(0, root.Length - 17);
            string path = root + @"\Tests";
            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(path);

            StorageFile sampleFile = await folder.GetFileAsync(file);
            var stream = await sampleFile.OpenStreamForReadAsync();

            content = new StreamContent(stream);
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "File",
                FileName = sampleFile.Name
            };
            form.Add(content);

            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

            requestUri = $"{apiRoute}" + "/add-photo";

            var result = await _client.PostAsync(requestUri, form);
            var messageJson = await result.Content.ReadAsStringAsync();
            var message = messageJson.Split(',');
            var id = message[0].Split("\"")[2].Split(":")[1];

            requestUri = $"{apiRoute}" + "/delete-photo/" + id;

            // Act
            httpResponse = await _client.DeleteAsync(requestUri);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("OK", "lisa", "Password")]
        public async Task GetUsersNoPagination_OK(string statusCode, string username, string password)
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = null;

            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
            requestUri = $"{apiRoute}";

            // Act
            httpResponse = await _client.GetAsync(requestUri);
            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("OK", "lisa", "Password", 1, 10)]
        public async Task GetUsersWithPagination_OK(string statusCode, string username, string password, int pageSize, int pageNumber)
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = null;

            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
            requestUri = $"{apiRoute}" + "?pageNumber=" + pageSize + "&pageSize=" + pageNumber;

            // Act
            httpResponse = await _client.GetAsync(requestUri);
            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("OK", "lisa", "Password", 18, 100)]
        public async Task GetUsersWithAge_OK(string statusCode, string username, string password, int minAge, int maxAge)
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = null;

            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
            requestUri = $"{apiRoute}" + "?MinAge=" + minAge + "&MaxAge=" + maxAge;

            // Act
            httpResponse = await _client.GetAsync(requestUri);
            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }


        [Theory]
        [InlineData("OK", "lisa", "Password")]
        public async Task GetUserByUsername_OK(string statusCode, string username, string password)
        {
            // Arrange
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
            requestUri = $"{apiRoute}/" + username;

            // Act
            httpResponse = await _client.GetAsync(requestUri);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("NoContent", "lisa", "Password", "IntroductionDiferente", "LookingForDiferente", "InterestsDiferente", "CityDiferente", "CountryDiferente")]
        public async Task UpdateUser_NoContent(string statusCode, string username, string password, string introduction, string lookingFor, string interests, string city, string country)
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = null;
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

            var memberUpdateDto = new MemberUpdateDTO
            {
                Introduction = introduction,
                LookingFor = lookingFor,
                Interests = interests,
                City = city,
                Country = country
            };
            registeredObject = GetRegisterObject(memberUpdateDto);
            httpContent = GetHttpContent(registeredObject);

            requestUri = $"{apiRoute}";

            // Act
            httpResponse = await _client.PutAsync(requestUri, httpContent);
            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("Created", "lisa", "Password", "foto1.png")]
        public async Task AddPhoto_Created(string statusCode, string username, string password, string file)
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = null;
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

            MultipartFormDataContent form = new MultipartFormDataContent();
            HttpContent content = new StringContent(file);
            form.Add(content, file);

            string root = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;
            root = root.Substring(0,root.Length-17);
            string path = root + @"\Tests";
            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(path);

            StorageFile sampleFile = await folder.GetFileAsync(file);
            var stream = await sampleFile.OpenStreamForReadAsync();
            content = new StreamContent(stream);
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "File",
                FileName = sampleFile.Name
            };
            form.Add(content);

            requestUri = $"{apiRoute}" + "/add-photo";

            // Act
            httpResponse = await _client.PostAsync(requestUri, form);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("NoContent", "lisa", "Password", "foto2.png")]
        public async Task SetMainPhoto_OK(string statusCode, string username, string password, string file)
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = null;
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

            MultipartFormDataContent form = new MultipartFormDataContent();
            HttpContent content = new StringContent(file);
            form.Add(content, file);
            string root = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;
            root = root.Substring(0, root.Length - 17);
            string path = root + @"\Tests";
            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(path);

            StorageFile sampleFile = await folder.GetFileAsync(file);
            var stream = await sampleFile.OpenStreamForReadAsync();
            content = new StreamContent(stream);
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "File",
                FileName = sampleFile.Name
            };
            form.Add(content);

            requestUri = $"{apiRoute}" + "/add-photo";

            var result = await _client.PostAsync(requestUri, form);
            var messageJson = await result.Content.ReadAsStringAsync();
            var message = messageJson.Split(',');
            var id = message[0].Split("\"")[2].Split(":")[1];

            requestUri = $"{apiRoute}" + "/set-main-photo/" + id;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
            // Act
            httpResponse = await _client.PutAsync(requestUri, null);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }


        [Theory]
        [InlineData("NotFound", "davis", "Password", "20")]
        public async Task DeletePhoto_NotFound(string statusCode, string username, string password, string id)
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = null;
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

            requestUri = $"{apiRoute}" + "/photos/" + id;

            // Act
            httpResponse = await _client.DeleteAsync(requestUri);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        #region Privated methods
        private static string GetRegisterObject(MemberUpdateDTO memberUpdateDto)
        {
            var entityObject = new JObject()
            {
                { nameof(memberUpdateDto.Introduction), memberUpdateDto.Introduction },
                { nameof(memberUpdateDto.LookingFor), memberUpdateDto.LookingFor },
                { nameof(memberUpdateDto.Interests), memberUpdateDto.Interests },
                { nameof(memberUpdateDto.City), memberUpdateDto.City },
                { nameof(memberUpdateDto.Country), memberUpdateDto.Country }
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
