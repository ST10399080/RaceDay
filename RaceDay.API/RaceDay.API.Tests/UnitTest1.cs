using RaceDay.API.Models.DTOs;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using RaceDay.API.Tests;

namespace RaceDay.API.Tests;

public class UnitTest1
{
    [Fact]
    public void EventCreateRequest_InvalidDistance_FailsValidation()
    {
        // Arrange
        var request = new EventCreateRequest
        {
            EventName = "Test Running Event",
            Description = "A test event for RaceDay.",
            EventDate = DateTime.Today.AddDays(7),
            Location = "Johannesburg",
            Distance = 0,
            EventType = "Run"
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(request);

        // Act
        var isValid = Validator.TryValidateObject(
            request,
            validationContext,
            validationResults,
            validateAllProperties: true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(
            validationResults,
            result => result.MemberNames.Contains("Distance"));
    }

    [Fact]
    public void EventCreateRequest_ValidData_PassesValidation()
    {
        // Arrange
        var request = new EventCreateRequest
        {
            EventName = "Johannesburg City Run",
            Description = "A community running event.",
            EventDate = DateTime.Today.AddDays(7),
            Location = "Johannesburg",
            Distance = 10,
            EventType = "Run"
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(request);

        // Act
        var isValid = Validator.TryValidateObject(
            request,
            validationContext,
            validationResults,
            validateAllProperties: true);

        // Assert
        Assert.True(isValid);
        Assert.Empty(validationResults);
    }

    [Fact]
    public void CategoryCreateRequest_EmptyName_FailsValidation()
    {
        // Arrange
        var request = new CategoryCreateRequest
        {
            CategoryName = ""
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(request);

        // Act
        var isValid = Validator.TryValidateObject(
            request,
            validationContext,
            validationResults,
            validateAllProperties: true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(
            validationResults,
            result => result.MemberNames.Contains("CategoryName"));
    }

    [Fact]
    public void EnrolmentCreateRequest_InvalidEventID_FailsValidation()
    {
        // Arrange
        var request = new EnrolmentCreateRequest
        {
            EventID = 0,
            CategoryID = 2
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(request);

        // Act
        var isValid = Validator.TryValidateObject(
            request,
            validationContext,
            validationResults,
            validateAllProperties: true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(
            validationResults,
            result => result.MemberNames.Contains("EventID"));
    }

    [Fact]
    public void ResultCreateRequest_InvalidPosition_FailsValidation()
    {
        // Arrange
        var request = new ResultCreateRequest
        {
            EnrolmentID = 1,
            FinishTime = TimeSpan.FromHours(1),
            Position = 0
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(request);

        // Act
        var isValid = Validator.TryValidateObject(
            request,
            validationContext,
            validationResults,
            validateAllProperties: true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(
            validationResults,
            result => result.MemberNames.Contains("Position"));
    }

    [Fact]
    public void ProfileUpdateRequest_InvalidEmail_FailsValidation()
    {
        // Arrange
        var request = new ProfileUpdateRequest
        {
            FirstName = "Thando",
            Surname = "Mokoena",
            Email = "not-an-email"
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(request);

        // Act
        var isValid = Validator.TryValidateObject(
            request,
            validationContext,
            validationResults,
            validateAllProperties: true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(
            validationResults,
            result => result.MemberNames.Contains("Email"));
    }

    [Fact]
    public void RegisterRequest_ShortPassword_FailsValidation()
    {
        // Arrange
        var request = new RegisterRequest
        {
            FirstName = "Test",
            Surname = "Participant",
            Email = "test.participant@example.com",
            Password = "abc123"
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(request);

        // Act
        var isValid = Validator.TryValidateObject(
            request,
            validationContext,
            validationResults,
            validateAllProperties: true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(
            validationResults,
            result => result.MemberNames.Contains("Password"));
    }
    [Fact]
    public void RegisterRequest_DoesNotExposeRoleProperty()
    {
        // Act
        var roleProperty = typeof(RegisterRequest).GetProperty("Role");

        // Assert
        Assert.Null(roleProperty);
    }

    [Fact]
    public void LoginRequest_InvalidEmail_FailsValidation()
    {
        // Arrange
        var request = new LoginRequest
        {
            Email = "not-an-email",
            Password = "ValidPassword123!"
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(request);

        // Act
        var isValid = Validator.TryValidateObject(
            request,
            validationContext,
            validationResults,
            validateAllProperties: true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(
            validationResults,
            result => result.MemberNames.Contains("Email"));
    }

    [Fact]
    public void RegisterRequest_LongPassword_FailsValidation()
    {
        // Arrange
        var request = new RegisterRequest
        {
            FirstName = "Test",
            Surname = "Participant",
            Email = "test.participant@example.com",
            Password = new string('A', 101)
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(request);

        // Act
        var isValid = Validator.TryValidateObject(
            request,
            validationContext,
            validationResults,
            validateAllProperties: true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(
            validationResults,
            result => result.MemberNames.Contains("Password"));
    }

    [Fact]
    public void LoginRequest_EmptyPassword_FailsValidation()
    {
        // Arrange
        var request = new LoginRequest
        {
            Email = "participant@example.com",
            Password = string.Empty
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(request);

        // Act
        var isValid = Validator.TryValidateObject(
            request,
            validationContext,
            validationResults,
            validateAllProperties: true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(
            validationResults,
            result => result.MemberNames.Contains("Password"));
    }

    [Fact]
    public void LoginRequest_EmptyEmail_FailsValidation()
    {
        // Arrange
        var request = new LoginRequest
        {
            Email = string.Empty,
            Password = "ValidPassword123!"
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(request);

        // Act
        var isValid = Validator.TryValidateObject(
            request,
            validationContext,
            validationResults,
            validateAllProperties: true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(
            validationResults,
            result => result.MemberNames.Contains("Email"));
    }

    [Fact]
    public void RegisterRequest_InvalidEmail_FailsValidation()
    {
        // Arrange
        var request = new RegisterRequest
        {
            FirstName = "Test",
            Surname = "Participant",
            Email = "invalid-email",
            Password = "ValidPassword123!"
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(request);

        // Act
        var isValid = Validator.TryValidateObject(
            request,
            validationContext,
            validationResults,
            validateAllProperties: true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(
            validationResults,
            result => result.MemberNames.Contains("Email"));
    }

    [Fact]
    public async Task Register_ValidRequest_ReturnsCreated()
    {
        // Arrange
        using var factory = new RaceDayWebApplicationFactory();
        using var client = factory.CreateClient();

        var request = new
        {
            FirstName = "Integration",
            Surname = "Participant",
            Email = $"participant{Guid.NewGuid():N}@example.com",
            Password = "ValidPassword123!"
        };

        // Act
        var response = await client.PostAsJsonAsync(
            "/api/Auth/register",
            request);

        // Assert
        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            response.StatusCode);
    }

    [Fact]
    public async Task Register_InvalidEmail_ReturnsBadRequest()
    {
        // Arrange
        using var factory = new RaceDayWebApplicationFactory();
        using var client = factory.CreateClient();

        var request = new
        {
            FirstName = "Integration",
            Surname = "Participant",
            Email = "invalid-email",
            Password = "ValidPassword123!"
        };

        // Act
        var response = await client.PostAsJsonAsync(
            "/api/Auth/register",
            request);

        // Assert
        Assert.Equal(
            System.Net.HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task Register_DuplicateEmail_ReturnsBadRequest()
    {
        // Arrange
        using var factory = new RaceDayWebApplicationFactory();
        using var client = factory.CreateClient();

        var email = $"duplicate{Guid.NewGuid():N}@example.com";

        var request = new
        {
            FirstName = "Test",
            Surname = "Participant",
            Email = email,
            Password = "ValidPassword123!"
        };

        // Act
        var firstResponse = await client.PostAsJsonAsync(
            "/api/Auth/register",
            request);

        var secondResponse = await client.PostAsJsonAsync(
            "/api/Auth/register",
            request);

        // Assert
        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            firstResponse.StatusCode);

        Assert.Equal(
            System.Net.HttpStatusCode.Conflict,
            secondResponse.StatusCode);
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsToken()
    {
        // Arrange
        using var factory = new RaceDayWebApplicationFactory();
        using var client = factory.CreateClient();

        var email = $"login{Guid.NewGuid():N}@example.com";
        var password = "ValidPassword123!";

        var registerRequest = new
        {
            FirstName = "Login",
            Surname = "Participant",
            Email = email,
            Password = password
        };

        var registerResponse = await client.PostAsJsonAsync(
            "/api/Auth/register",
            registerRequest);

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            registerResponse.StatusCode);

        var loginRequest = new
        {
            Email = email,
            Password = password
        };

        // Act
        var loginResponse = await client.PostAsJsonAsync(
            "/api/Auth/login",
            loginRequest);

        var responseBody =
            await loginResponse.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(
            System.Net.HttpStatusCode.OK,
            loginResponse.StatusCode);

        Assert.Contains("accessToken", responseBody);
        Assert.Contains("Bearer", responseBody);
    }

    [Fact]
    public async Task Login_InvalidPassword_ReturnsUnauthorized()
    {
        // Arrange
        using var factory = new RaceDayWebApplicationFactory();
        using var client = factory.CreateClient();

        var email = $"wrongpassword{Guid.NewGuid():N}@example.com";

        var registerRequest = new
        {
            FirstName = "Test",
            Surname = "Participant",
            Email = email,
            Password = "CorrectPassword123!"
        };

        var registerResponse = await client.PostAsJsonAsync(
            "/api/Auth/register",
            registerRequest);

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            registerResponse.StatusCode);

        var loginRequest = new
        {
            Email = email,
            Password = "WrongPassword123!"
        };

        // Act
        var loginResponse = await client.PostAsJsonAsync(
            "/api/Auth/login",
            loginRequest);

        // Assert
        Assert.Equal(
            System.Net.HttpStatusCode.Unauthorized,
            loginResponse.StatusCode);
    }

    [Fact]
    public async Task Login_UnknownEmail_ReturnsUnauthorized()
    {
        // Arrange
        using var factory = new RaceDayWebApplicationFactory();
        using var client = factory.CreateClient();

        var loginRequest = new
        {
            Email = $"unknown{Guid.NewGuid():N}@example.com",
            Password = "ValidPassword123!"
        };

        // Act
        var response = await client.PostAsJsonAsync(
            "/api/Auth/login",
            loginRequest);

        // Assert
        Assert.Equal(
            System.Net.HttpStatusCode.Unauthorized,
            response.StatusCode);
    }
}
