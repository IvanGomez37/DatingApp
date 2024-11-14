namespace API.UnitTests.Tests;
using System.Text;
using API.DTOs;
using API.UnitTests.Helpers;
using Newtonsoft.Json.Linq;

public class AccountControllerTests
{
    private string apiRoute = "api/account";
    private readonly HttpClient _client;
    private HttpResponseMessage httpResponse;
    private string requestUrl;
    private string registerObjetct;
    private string loginObjetct;
    private HttpContent httpContent;
    
    public AccountControllerTests()
    {
        _client = TestHelper.Instance.Client;
        httpResponse = new HttpResponseMessage();
        requestUrl = string.Empty;
        registerObjetct = string.Empty;
        loginObjetct = string.Empty;
        httpContent = new StringContent(string.Empty);
    }

    [Theory]
    [InlineData("OK", "Angel", "Prueba12")]
    public async Task RegisterUserOK(string statusCode, string usernamex, string passwordx)
    {
        // Arrange
        requestUrl = $"{apiRoute}/register";
        var registerRequest = new RegisterRequest
        {
            username = usernamex,
            password = passwordx
        };

        registerObjetct = GetRegisterObject(registerRequest);
        httpContent = GetHttpContent(registerObjetct);
        httpResponse = await _client.PostAsync(requestUrl, httpContent);
        var reponse = await httpResponse.Content.ReadAsStringAsync();
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("BadRequest", "Bob", "Prueba12")]
    public async Task RegisterUserBR(string statusCode, string usernamex, string passwordx)
    {
        // Arrange
        requestUrl = $"{apiRoute}/register";
        var registerRequest = new RegisterRequest
        {
            username = usernamex,
            password = passwordx
        };

        registerObjetct = GetRegisterObject(registerRequest);
        httpContent = GetHttpContent(registerObjetct);
        httpResponse = await _client.PostAsync(requestUrl, httpContent);
        var reponse = await httpResponse.Content.ReadAsStringAsync();
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("OK", "Bob", "HolaMundo")]
    public async Task LoginrUserOK(string statusCode, string usernamex, string passwordx)
    {
        // Arrange
        requestUrl = $"{apiRoute}/login";
        var loginRequest = new LoginRequest
        {
            username = usernamex,
            password = passwordx
        };

        loginObjetct = GetLoginObject(loginRequest);
        httpContent = GetHttpContent(loginObjetct);
        httpResponse = await _client.PostAsync(requestUrl, httpContent);
        var reponse = await httpResponse.Content.ReadAsStringAsync();
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("Unauthorized", "Francisco", "HolaMundo")]
    public async Task LoginrUserUN1(string statusCode, string usernamex, string passwordx)
    {
        // Arrange
        requestUrl = $"{apiRoute}/login";
        var loginRequest = new LoginRequest
        {
            username = usernamex,
            password = passwordx
        };

        loginObjetct = GetLoginObject(loginRequest);
        httpContent = GetHttpContent(loginObjetct);
        httpResponse = await _client.PostAsync(requestUrl, httpContent);
        var reponse = await httpResponse.Content.ReadAsStringAsync();
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("Unauthorized", "Bob", "HolaMundoError")]
    public async Task LoginrUserUN2(string statusCode, string usernamex, string passwordx)
    {
        // Arrange
        requestUrl = $"{apiRoute}/login";
        var loginRequest = new LoginRequest
        {
            username = usernamex,
            password = passwordx
        };

        loginObjetct = GetLoginObject(loginRequest);
        httpContent = GetHttpContent(loginObjetct);
        httpResponse = await _client.PostAsync(requestUrl, httpContent);
        var reponse = await httpResponse.Content.ReadAsStringAsync();
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

/*     [Theory]
    [InlineData("NotFound")]
    public async Task GetNotFoundShouldNotFound(string statusCode)
    {
        // Arrange
        requestUrl = $"{apiRoute}/not-found";
        // Act
        httpResponse = await _client.GetAsync(requestUrl);
        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("InternalServerError")]
    public async Task GetServerErrorShouldNotInternalServerError(string statusCode)
    {
        // Arrange
        requestUrl = $"{apiRoute}/server-error";
        // Act
        httpResponse = await _client.GetAsync(requestUrl);
        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("BadRequest")]
    public async Task GetBadRequestShouldBadRequest(string statusCode)
    {
        // Arrange
        requestUrl = $"{apiRoute}/bad-request";
        // Act
        httpResponse = await _client.GetAsync(requestUrl);
        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    } */

    #region Privated methods
    private static string GetRegisterObject(RegisterRequest registerDto)
    {
        var entityObject = new JObject()
            {
                { nameof(registerDto.username), registerDto.username },
                { nameof(registerDto.password), registerDto.password }
            };
        return entityObject.ToString();
    }

    private static string GetLoginObject(LoginRequest loginDto)
    {
        var entityObject = new JObject()
            {
                { nameof(loginDto.username), loginDto.username },
                { nameof(loginDto.password), loginDto.password }
            };
        return entityObject.ToString();
    }

    private static StringContent GetHttpContent(string objectToCode)
    {
        return new StringContent(objectToCode, Encoding.UTF8, "application/json");
    }
    #endregion
}