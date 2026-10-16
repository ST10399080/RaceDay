using RaceDay.API.Models.DTOs;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Net.Http.Headers;
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

    [Fact]
    public async Task Participant_CannotAccessOrganiserEndpoint_ReturnsForbidden()
    {
        // Arrange
        using var factory = new RaceDayWebApplicationFactory();
        using var client = factory.CreateClient();

        var email = $"rolecheck{Guid.NewGuid():N}@example.com";
        var password = "ValidPassword123!";

        var registerResponse = await client.PostAsJsonAsync(
            "/api/Auth/register",
            new
            {
                FirstName = "Test",
                Surname = "Participant",
                Email = email,
                Password = password
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            registerResponse.StatusCode);

        var loginResponse = await client.PostAsJsonAsync(
            "/api/Auth/login",
            new
            {
                Email = email,
                Password = password
            });

        Assert.Equal(
            System.Net.HttpStatusCode.OK,
            loginResponse.StatusCode);

        var loginBody = await loginResponse.Content
            .ReadFromJsonAsync<LoginResponse>();

        Assert.NotNull(loginBody);
        Assert.False(string.IsNullOrWhiteSpace(loginBody.AccessToken));

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer",
                loginBody.AccessToken);

        // Act
        var response = await client.GetAsync(
            "/api/TestAccess/organiser");

        // Assert
        Assert.Equal(
            System.Net.HttpStatusCode.Forbidden,
            response.StatusCode);
    }

    private sealed class LoginResponse
    {
        public string AccessToken { get; set; } = string.Empty;
    }

    private sealed class EventCreationResponse
    {
        public int EventID { get; set; }
    }

    [Fact]
    public async Task UnauthenticatedUser_CannotAccessProtectedEndpoint_ReturnsUnauthorized()
    {
        // Arrange
        using var factory = new RaceDayWebApplicationFactory();
        using var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync(
            "/api/TestAccess/authenticated");

        // Assert
        Assert.Equal(
            System.Net.HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task Participant_CanAccessParticipantEndpoint_ReturnsOk()
    {
        // Arrange
        using var factory = new RaceDayWebApplicationFactory();
        using var client = factory.CreateClient();

        var email = $"participant{Guid.NewGuid():N}@example.com";
        var password = "ValidPassword123!";

        var registerResponse = await client.PostAsJsonAsync(
            "/api/Auth/register",
            new
            {
                FirstName = "Test",
                Surname = "Participant",
                Email = email,
                Password = password
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            registerResponse.StatusCode);

        var loginResponse = await client.PostAsJsonAsync(
            "/api/Auth/login",
            new
            {
                Email = email,
                Password = password
            });

        Assert.Equal(
            System.Net.HttpStatusCode.OK,
            loginResponse.StatusCode);

        var loginBody = await loginResponse.Content
            .ReadFromJsonAsync<LoginResponse>();

        Assert.NotNull(loginBody);
        Assert.False(string.IsNullOrWhiteSpace(loginBody.AccessToken));

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer",
                loginBody.AccessToken);

        // Act
        var response = await client.GetAsync(
            "/api/TestAccess/participant");

        // Assert
        Assert.Equal(
            System.Net.HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task Participant_CannotCreateEvent_ReturnsForbidden()
    {
        // Arrange
        using var factory = new RaceDayWebApplicationFactory();
        using var client = factory.CreateClient();

        var email = $"eventtest{Guid.NewGuid():N}@example.com";
        var password = "ValidPassword123!";

        var registerResponse = await client.PostAsJsonAsync(
            "/api/Auth/register",
            new
            {
                FirstName = "Test",
                Surname = "Participant",
                Email = email,
                Password = password
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            registerResponse.StatusCode);

        var loginResponse = await client.PostAsJsonAsync(
            "/api/Auth/login",
            new
            {
                Email = email,
                Password = password
            });

        Assert.Equal(
            System.Net.HttpStatusCode.OK,
            loginResponse.StatusCode);

        var loginBody = await loginResponse.Content
            .ReadFromJsonAsync<LoginResponse>();

        Assert.NotNull(loginBody);
        Assert.False(string.IsNullOrWhiteSpace(loginBody.AccessToken));

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer",
                loginBody.AccessToken);

        var eventRequest = new
        {
            EventName = "Test Charity Run",
            Description = "An integration test event.",
            EventDate = DateTime.Today.AddDays(30),
            Location = "Johannesburg",
            Distance = 5.0m,
            EventType = "Run",
            BannerImageURL = (string?)null
        };

        // Act
        var response = await client.PostAsJsonAsync(
            "/api/Events",
            eventRequest);

        // Assert
        Assert.Equal(
            System.Net.HttpStatusCode.Forbidden,
            response.StatusCode);
    }

    [Fact]
    public async Task Participant_CanViewEvents_ReturnsOk()
    {
        // Arrange
        using var factory = new RaceDayWebApplicationFactory();
        using var client = factory.CreateClient();

        var email = $"viewevents{Guid.NewGuid():N}@example.com";
        var password = "ValidPassword123!";

        var registerResponse = await client.PostAsJsonAsync(
            "/api/Auth/register",
            new
            {
                FirstName = "Test",
                Surname = "Participant",
                Email = email,
                Password = password
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            registerResponse.StatusCode);

        var loginResponse = await client.PostAsJsonAsync(
            "/api/Auth/login",
            new
            {
                Email = email,
                Password = password
            });

        Assert.Equal(
            System.Net.HttpStatusCode.OK,
            loginResponse.StatusCode);

        var loginBody = await loginResponse.Content
            .ReadFromJsonAsync<LoginResponse>();

        Assert.NotNull(loginBody);
        Assert.False(string.IsNullOrWhiteSpace(loginBody.AccessToken));

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer",
                loginBody.AccessToken);

        // Act
        var response = await client.GetAsync("/api/Events");

        // Assert
        Assert.Equal(
            System.Net.HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task UnauthenticatedUser_CannotViewEvents_ReturnsUnauthorized()
    {
        // Arrange
        using var factory = new RaceDayWebApplicationFactory();
        using var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/Events");

        // Assert
        Assert.Equal(
            System.Net.HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task Organiser_CanCreateEvent_ReturnsCreated()
    {
        // Arrange
        using var factory = new RaceDayWebApplicationFactory();
        using var client = factory.CreateClient();

        var email = $"organiser{Guid.NewGuid():N}@example.com";
        var password = "ValidPassword123!";

        var organiserResponse = await client.PostAsJsonAsync(
            "/api/Auth/create-organiser-dev",
            new
            {
                FirstName = "Test",
                Surname = "Organiser",
                Email = email,
                Password = password
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            organiserResponse.StatusCode);

        var loginResponse = await client.PostAsJsonAsync(
            "/api/Auth/login",
            new
            {
                Email = email,
                Password = password
            });

        Assert.Equal(
            System.Net.HttpStatusCode.OK,
            loginResponse.StatusCode);

        var loginBody = await loginResponse.Content
            .ReadFromJsonAsync<LoginResponse>();

        Assert.NotNull(loginBody);
        Assert.False(string.IsNullOrWhiteSpace(loginBody.AccessToken));

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer",
                loginBody.AccessToken);

        var eventRequest = new
        {
            EventName = "Test Charity Run",
            Description = "An integration test event.",
            EventDate = DateTime.Today.AddDays(30),
            Location = "Johannesburg",
            Distance = 5.0m,
            EventType = "Run",
            BannerImageURL = (string?)null
        };

        // Act
        var response = await client.PostAsJsonAsync(
            "/api/Events",
            eventRequest);

        // Assert
        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            response.StatusCode);
    }

    [Fact]
    public async Task Organiser_CannotUpdateAnotherOrganisersEvent_ReturnsForbidden()
    {
        using var factory = new RaceDayWebApplicationFactory();
        using var client = factory.CreateClient();

        // Create Organiser A
        var organiserAEmail = $"organisera{Guid.NewGuid():N}@example.com";
        const string password = "ValidPassword123!";

        var createA = await client.PostAsJsonAsync(
            "/api/Auth/create-organiser-dev",
            new
            {
                FirstName = "Organiser",
                Surname = "A",
                Email = organiserAEmail,
                Password = password
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            createA.StatusCode);

        // Log in as Organiser A
        var loginA = await client.PostAsJsonAsync(
            "/api/Auth/login",
            new { Email = organiserAEmail, Password = password });

        Assert.Equal(System.Net.HttpStatusCode.OK, loginA.StatusCode);

        var tokenA = await loginA.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.NotNull(tokenA);

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer", tokenA.AccessToken);

        // Organiser A creates an event
        var createEvent = await client.PostAsJsonAsync(
            "/api/Events",
            new
            {
                EventName = "Organiser A Charity Run",
                Description = "An event owned by Organiser A.",
                EventDate = DateTime.Today.AddDays(30),
                Location = "Johannesburg",
                Distance = 5.0m,
                EventType = "Run",
                BannerImageURL = (string?)null
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            createEvent.StatusCode);

        var createdEvent = await createEvent.Content
            .ReadFromJsonAsync<EventCreationResponse>();

        Assert.NotNull(createdEvent);
        Assert.True(createdEvent.EventID > 0);

        // Create Organiser B
        var organiserBEmail = $"organiserb{Guid.NewGuid():N}@example.com";

        var createB = await client.PostAsJsonAsync(
            "/api/Auth/create-organiser-dev",
            new
            {
                FirstName = "Organiser",
                Surname = "B",
                Email = organiserBEmail,
                Password = password
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            createB.StatusCode);

        // Log in as Organiser B
        var loginB = await client.PostAsJsonAsync(
            "/api/Auth/login",
            new { Email = organiserBEmail, Password = password });

        Assert.Equal(System.Net.HttpStatusCode.OK, loginB.StatusCode);

        var tokenB = await loginB.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.NotNull(tokenB);

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer", tokenB.AccessToken);

        // Organiser B attempts to update Organiser A's event
        var updateResponse = await client.PutAsJsonAsync(
            $"/api/Events/{createdEvent.EventID}",
            new
            {
                EventName = "Unauthorised Event Change",
                Description = "This update should be rejected.",
                EventDate = DateTime.Today.AddDays(45),
                Location = "Pretoria",
                Distance = 10.0m,
                EventType = "Run",
                BannerImageURL = (string?)null
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Forbidden,
            updateResponse.StatusCode);
    }

    [Fact]
    public async Task Organiser_CannotDeleteAnotherOrganisersEvent_ReturnsForbidden()
    {
        using var factory = new RaceDayWebApplicationFactory();
        using var client = factory.CreateClient();

        const string password = "ValidPassword123!";

        // Create Organiser A
        var emailA = $"deleteowner{Guid.NewGuid():N}@example.com";

        var createA = await client.PostAsJsonAsync(
            "/api/Auth/create-organiser-dev",
            new
            {
                FirstName = "Organiser",
                Surname = "A",
                Email = emailA,
                Password = password
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            createA.StatusCode);

        // Log in as Organiser A
        var loginA = await client.PostAsJsonAsync(
            "/api/Auth/login",
            new { Email = emailA, Password = password });

        Assert.Equal(System.Net.HttpStatusCode.OK, loginA.StatusCode);

        var tokenA = await loginA.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.NotNull(tokenA);

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer", tokenA.AccessToken);

        // Organiser A creates an event
        var createEvent = await client.PostAsJsonAsync(
            "/api/Events",
            new
            {
                EventName = "Protected Charity Walk",
                Description = "An event owned by Organiser A.",
                EventDate = DateTime.Today.AddDays(30),
                Location = "Johannesburg",
                Distance = 5.0m,
                EventType = "Walk",
                BannerImageURL = (string?)null
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            createEvent.StatusCode);

        var createdEvent = await createEvent.Content
            .ReadFromJsonAsync<EventCreationResponse>();

        Assert.NotNull(createdEvent);
        Assert.True(createdEvent.EventID > 0);

        // Create Organiser B
        var emailB = $"deleteattempt{Guid.NewGuid():N}@example.com";

        var createB = await client.PostAsJsonAsync(
            "/api/Auth/create-organiser-dev",
            new
            {
                FirstName = "Organiser",
                Surname = "B",
                Email = emailB,
                Password = password
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            createB.StatusCode);

        // Log in as Organiser B
        var loginB = await client.PostAsJsonAsync(
            "/api/Auth/login",
            new { Email = emailB, Password = password });

        Assert.Equal(System.Net.HttpStatusCode.OK, loginB.StatusCode);

        var tokenB = await loginB.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.NotNull(tokenB);

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer", tokenB.AccessToken);

        // Organiser B attempts to delete Organiser A's event
        var deleteResponse = await client.DeleteAsync(
            $"/api/Events/{createdEvent.EventID}");

        Assert.Equal(
            System.Net.HttpStatusCode.Forbidden,
            deleteResponse.StatusCode);
    }

    [Fact]
    public async Task Organiser_CanUpdateOwnEvent_ReturnsOk()
    {
        using var factory = new RaceDayWebApplicationFactory();
        using var client = factory.CreateClient();

        var email = $"updateown{Guid.NewGuid():N}@example.com";
        const string password = "ValidPassword123!";

        // Create an Organiser
        var createOrganiser = await client.PostAsJsonAsync(
            "/api/Auth/create-organiser-dev",
            new
            {
                FirstName = "Test",
                Surname = "Organiser",
                Email = email,
                Password = password
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            createOrganiser.StatusCode);

        // Log in
        var login = await client.PostAsJsonAsync(
            "/api/Auth/login",
            new { Email = email, Password = password });

        Assert.Equal(System.Net.HttpStatusCode.OK, login.StatusCode);

        var loginBody = await login.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.NotNull(loginBody);

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer", loginBody.AccessToken);

        // Create an event
        var createEvent = await client.PostAsJsonAsync(
            "/api/Events",
            new
            {
                EventName = "Original Charity Run",
                Description = "Original event description.",
                EventDate = DateTime.Today.AddDays(30),
                Location = "Johannesburg",
                Distance = 5.0m,
                EventType = "Run",
                BannerImageURL = (string?)null
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            createEvent.StatusCode);

        var createdEvent = await createEvent.Content
            .ReadFromJsonAsync<EventCreationResponse>();

        Assert.NotNull(createdEvent);
        Assert.True(createdEvent.EventID > 0);

        // Update the Organiser's own event
        var updateResponse = await client.PutAsJsonAsync(
            $"/api/Events/{createdEvent.EventID}",
            new
            {
                EventName = "Updated Charity Run",
                Description = "Updated event description.",
                EventDate = DateTime.Today.AddDays(45),
                Location = "Pretoria",
                Distance = 10.0m,
                EventType = "Run",
                BannerImageURL = (string?)null
            });

        Assert.Equal(
            System.Net.HttpStatusCode.OK,
            updateResponse.StatusCode);
    }

    [Fact]
    public async Task Organiser_CanDeleteOwnEvent_ReturnsNoContent()
    {
        using var factory = new RaceDayWebApplicationFactory();
        using var client = factory.CreateClient();

        var email = $"deleteown{Guid.NewGuid():N}@example.com";
        const string password = "ValidPassword123!";

        // Create an Organiser
        var createOrganiser = await client.PostAsJsonAsync(
            "/api/Auth/create-organiser-dev",
            new
            {
                FirstName = "Test",
                Surname = "Organiser",
                Email = email,
                Password = password
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            createOrganiser.StatusCode);

        // Log in
        var login = await client.PostAsJsonAsync(
            "/api/Auth/login",
            new { Email = email, Password = password });

        Assert.Equal(System.Net.HttpStatusCode.OK, login.StatusCode);

        var loginBody = await login.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.NotNull(loginBody);

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer", loginBody.AccessToken);

        // Create an event
        var createEvent = await client.PostAsJsonAsync(
            "/api/Events",
            new
            {
                EventName = "Event to Delete",
                Description = "Testing event deletion by its owner.",
                EventDate = DateTime.Today.AddDays(30),
                Location = "Johannesburg",
                Distance = 5.0m,
                EventType = "Walk",
                BannerImageURL = (string?)null
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            createEvent.StatusCode);

        var createdEvent = await createEvent.Content
            .ReadFromJsonAsync<EventCreationResponse>();

        Assert.NotNull(createdEvent);
        Assert.True(createdEvent.EventID > 0);

        // Delete the Organiser's own event
        var deleteResponse = await client.DeleteAsync(
            $"/api/Events/{createdEvent.EventID}");

        Assert.Equal(
            System.Net.HttpStatusCode.NoContent,
            deleteResponse.StatusCode);
    }

    [Fact]
    public async Task Organiser_CanCreateAndViewOwnEventCategory_ReturnsSuccess()
    {
        using var factory = new RaceDayWebApplicationFactory();
        using var client = factory.CreateClient();

        var email = $"categorytest{Guid.NewGuid():N}@example.com";
        const string password = "ValidPassword123!";

        // Create an Organiser
        var createOrganiser = await client.PostAsJsonAsync(
            "/api/Auth/create-organiser-dev",
            new
            {
                FirstName = "Test",
                Surname = "Organiser",
                Email = email,
                Password = password
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            createOrganiser.StatusCode);

        // Log in
        var login = await client.PostAsJsonAsync(
            "/api/Auth/login",
            new { Email = email, Password = password });

        Assert.Equal(System.Net.HttpStatusCode.OK, login.StatusCode);

        var loginBody = await login.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.NotNull(loginBody);

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer", loginBody.AccessToken);

        // Create an event
        var createEvent = await client.PostAsJsonAsync(
            "/api/Events",
            new
            {
                EventName = "Category Test Run",
                Description = "An event for category integration testing.",
                EventDate = DateTime.Today.AddDays(30),
                Location = "Johannesburg",
                Distance = 10.0m,
                EventType = "Run",
                BannerImageURL = (string?)null
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            createEvent.StatusCode);

        var createdEvent = await createEvent.Content
            .ReadFromJsonAsync<EventCreationResponse>();

        Assert.NotNull(createdEvent);
        Assert.True(createdEvent.EventID > 0);

        // Create a category for the event
        var createCategory = await client.PostAsJsonAsync(
            $"/api/Categories/event/{createdEvent.EventID}",
            new
            {
                CategoryName = "Open - 10 km"
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            createCategory.StatusCode);

        // View categories for the event
        var getCategories = await client.GetAsync(
            $"/api/Categories/event/{createdEvent.EventID}");

        Assert.Equal(
            System.Net.HttpStatusCode.OK,
            getCategories.StatusCode);
    }

    [Fact]
    public async Task Participant_CannotCreateCategory_ReturnsForbidden()
    {
        using var factory = new RaceDayWebApplicationFactory();
        using var client = factory.CreateClient();

        var email = $"participantcategory{Guid.NewGuid():N}@example.com";
        const string password = "ValidPassword123!";

        // Register a Participant
        var registerResponse = await client.PostAsJsonAsync(
            "/api/Auth/register",
            new
            {
                FirstName = "Test",
                Surname = "Participant",
                Email = email,
                Password = password
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            registerResponse.StatusCode);

        // Log in as the Participant
        var loginResponse = await client.PostAsJsonAsync(
            "/api/Auth/login",
            new { Email = email, Password = password });

        Assert.Equal(
            System.Net.HttpStatusCode.OK,
            loginResponse.StatusCode);

        var loginBody = await loginResponse.Content
            .ReadFromJsonAsync<LoginResponse>();

        Assert.NotNull(loginBody);

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer", loginBody.AccessToken);

        // Attempt to create a category
        // Event ID 1 does not need to exist because authorization
        // should reject the Participant before the action executes.
        var response = await client.PostAsJsonAsync(
            "/api/Categories/event/1",
            new
            {
                CategoryName = "Unauthorised Category"
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Forbidden,
            response.StatusCode);
    }

    [Fact]
    public async Task Participant_CanEnrolInEvent_ReturnsCreated()
    {
        using var factory = new RaceDayWebApplicationFactory();
        using var client = factory.CreateClient();

        const string password = "ValidPassword123!";

        // Create an Organiser
        var organiserEmail = $"enrolorganiser{Guid.NewGuid():N}@example.com";

        var createOrganiser = await client.PostAsJsonAsync(
            "/api/Auth/create-organiser-dev",
            new
            {
                FirstName = "Test",
                Surname = "Organiser",
                Email = organiserEmail,
                Password = password
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            createOrganiser.StatusCode);

        // Log in as Organiser
        var organiserLogin = await client.PostAsJsonAsync(
            "/api/Auth/login",
            new { Email = organiserEmail, Password = password });

        Assert.Equal(System.Net.HttpStatusCode.OK, organiserLogin.StatusCode);

        var organiserToken = await organiserLogin.Content
            .ReadFromJsonAsync<LoginResponse>();

        Assert.NotNull(organiserToken);

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer", organiserToken.AccessToken);

        // Create an event
        var createEvent = await client.PostAsJsonAsync(
            "/api/Events",
            new
            {
                EventName = "Enrolment Test Run",
                Description = "An event for enrolment integration testing.",
                EventDate = DateTime.Today.AddDays(30),
                Location = "Johannesburg",
                Distance = 5.0m,
                EventType = "Run",
                BannerImageURL = (string?)null
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            createEvent.StatusCode);

        var createdEvent = await createEvent.Content
            .ReadFromJsonAsync<EventCreationResponse>();

        Assert.NotNull(createdEvent);
        Assert.True(createdEvent.EventID > 0);

        // Create a category for the event
        var createCategory = await client.PostAsJsonAsync(
            $"/api/Categories/event/{createdEvent.EventID}",
            new { CategoryName = "Open - 5 km" });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            createCategory.StatusCode);

        // Retrieve the category ID
        var categoriesResponse = await client.GetAsync(
            $"/api/Categories/event/{createdEvent.EventID}");

        Assert.Equal(
            System.Net.HttpStatusCode.OK,
            categoriesResponse.StatusCode);

        var categories = await categoriesResponse.Content
            .ReadFromJsonAsync<List<CategoryResponse>>();

        Assert.NotNull(categories);

        var category = categories.FirstOrDefault(
            c => c.CategoryName == "Open - 5 km");

        Assert.NotNull(category);
        Assert.True(category.CategoryID > 0);

        // Register a Participant
        var participantEmail = $"enrolparticipant{Guid.NewGuid():N}@example.com";

        var registerParticipant = await client.PostAsJsonAsync(
            "/api/Auth/register",
            new
            {
                FirstName = "Test",
                Surname = "Participant",
                Email = participantEmail,
                Password = password
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            registerParticipant.StatusCode);

        // Log in as Participant
        var participantLogin = await client.PostAsJsonAsync(
            "/api/Auth/login",
            new { Email = participantEmail, Password = password });

        Assert.Equal(
            System.Net.HttpStatusCode.OK,
            participantLogin.StatusCode);

        var participantToken = await participantLogin.Content
            .ReadFromJsonAsync<LoginResponse>();

        Assert.NotNull(participantToken);

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer", participantToken.AccessToken);

        // Enrol in the event using the selected category
        var enrolmentResponse = await client.PostAsJsonAsync(
            "/api/Enrolments",
            new
            {
                EventID = createdEvent.EventID,
                CategoryID = category.CategoryID
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            enrolmentResponse.StatusCode);
    }
    private sealed class CategoryResponse
    {
        public int CategoryID { get; set; }
        public int EventID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }

    [Fact]
    public async Task Participant_CannotEnrolInSameEventTwice_ReturnsConflict()
    {
        using var factory = new RaceDayWebApplicationFactory();
        using var client = factory.CreateClient();

        const string password = "ValidPassword123!";

        // Create an Organiser
        var organiserEmail = $"duplicateorg{Guid.NewGuid():N}@example.com";

        var createOrganiser = await client.PostAsJsonAsync(
            "/api/Auth/create-organiser-dev",
            new
            {
                FirstName = "Test",
                Surname = "Organiser",
                Email = organiserEmail,
                Password = password
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            createOrganiser.StatusCode);

        // Log in as Organiser
        var organiserLogin = await client.PostAsJsonAsync(
            "/api/Auth/login",
            new { Email = organiserEmail, Password = password });

        Assert.Equal(System.Net.HttpStatusCode.OK, organiserLogin.StatusCode);

        var organiserToken = await organiserLogin.Content
            .ReadFromJsonAsync<LoginResponse>();

        Assert.NotNull(organiserToken);

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer", organiserToken.AccessToken);

        // Create an event
        var createEvent = await client.PostAsJsonAsync(
            "/api/Events",
            new
            {
                EventName = "Duplicate Enrolment Test",
                Description = "Testing duplicate enrolment prevention.",
                EventDate = DateTime.Today.AddDays(30),
                Location = "Johannesburg",
                Distance = 5.0m,
                EventType = "Run",
                BannerImageURL = (string?)null
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            createEvent.StatusCode);

        var createdEvent = await createEvent.Content
            .ReadFromJsonAsync<EventCreationResponse>();

        Assert.NotNull(createdEvent);

        // Create a category
        var createCategory = await client.PostAsJsonAsync(
            $"/api/Categories/event/{createdEvent.EventID}",
            new { CategoryName = "Open - 5 km" });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            createCategory.StatusCode);

        var categoriesResponse = await client.GetAsync(
            $"/api/Categories/event/{createdEvent.EventID}");

        Assert.Equal(
            System.Net.HttpStatusCode.OK,
            categoriesResponse.StatusCode);

        var categories = await categoriesResponse.Content
            .ReadFromJsonAsync<List<CategoryResponse>>();

        Assert.NotNull(categories);

        var category = categories.FirstOrDefault(
            c => c.CategoryName == "Open - 5 km");

        Assert.NotNull(category);

        // Register a Participant
        var participantEmail = $"duplicateparticipant{Guid.NewGuid():N}@example.com";

        var registerParticipant = await client.PostAsJsonAsync(
            "/api/Auth/register",
            new
            {
                FirstName = "Test",
                Surname = "Participant",
                Email = participantEmail,
                Password = password
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            registerParticipant.StatusCode);

        // Log in as Participant
        var participantLogin = await client.PostAsJsonAsync(
            "/api/Auth/login",
            new { Email = participantEmail, Password = password });

        Assert.Equal(
            System.Net.HttpStatusCode.OK,
            participantLogin.StatusCode);

        var participantToken = await participantLogin.Content
            .ReadFromJsonAsync<LoginResponse>();

        Assert.NotNull(participantToken);

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer", participantToken.AccessToken);

        var enrolmentRequest = new
        {
            EventID = createdEvent.EventID,
            CategoryID = category.CategoryID
        };

        // First enrolment should succeed
        var firstEnrolment = await client.PostAsJsonAsync(
            "/api/Enrolments",
            enrolmentRequest);

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            firstEnrolment.StatusCode);

        // Second enrolment for the same event should be rejected
        var secondEnrolment = await client.PostAsJsonAsync(
            "/api/Enrolments",
            enrolmentRequest);

        Assert.Equal(
            System.Net.HttpStatusCode.Conflict,
            secondEnrolment.StatusCode);
    }

    [Fact]
    public async Task Participant_CannotRecordResult_ReturnsForbidden()
    {
        using var factory = new RaceDayWebApplicationFactory();
        using var client = factory.CreateClient();

        var email = $"resultparticipant{Guid.NewGuid():N}@example.com";
        const string password = "ValidPassword123!";

        // Register a Participant
        var registerResponse = await client.PostAsJsonAsync(
            "/api/Auth/register",
            new
            {
                FirstName = "Test",
                Surname = "Participant",
                Email = email,
                Password = password
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            registerResponse.StatusCode);

        // Log in as Participant
        var loginResponse = await client.PostAsJsonAsync(
            "/api/Auth/login",
            new { Email = email, Password = password });

        Assert.Equal(
            System.Net.HttpStatusCode.OK,
            loginResponse.StatusCode);

        var loginBody = await loginResponse.Content
            .ReadFromJsonAsync<LoginResponse>();

        Assert.NotNull(loginBody);

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer", loginBody.AccessToken);

        // Attempt to record a result
        var resultResponse = await client.PostAsJsonAsync(
            "/api/Results",
            new
            {
                EnrolmentID = 1,
                FinishTime = "01:15:30",
                Position = 1
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Forbidden,
            resultResponse.StatusCode);
    }

    [Fact]
    public async Task Participant_CanViewOwnResults_ReturnsOk()
    {
        using var factory = new RaceDayWebApplicationFactory();
        using var client = factory.CreateClient();

        var email = $"myresults{Guid.NewGuid():N}@example.com";
        const string password = "ValidPassword123!";

        // Register a Participant
        var registerResponse = await client.PostAsJsonAsync(
            "/api/Auth/register",
            new
            {
                FirstName = "Test",
                Surname = "Participant",
                Email = email,
                Password = password
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            registerResponse.StatusCode);

        // Log in as Participant
        var loginResponse = await client.PostAsJsonAsync(
            "/api/Auth/login",
            new { Email = email, Password = password });

        Assert.Equal(
            System.Net.HttpStatusCode.OK,
            loginResponse.StatusCode);

        var loginBody = await loginResponse.Content
            .ReadFromJsonAsync<LoginResponse>();

        Assert.NotNull(loginBody);

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer", loginBody.AccessToken);

        // Retrieve the Participant's results
        var resultsResponse = await client.GetAsync(
            "/api/Results/my-results");

        Assert.Equal(
            System.Net.HttpStatusCode.OK,
            resultsResponse.StatusCode);
    }

    [Fact]
    public async Task Participant_CanOnlyAccessOwnProfile_ReturnsOwnDetails()
    {
        using var factory = new RaceDayWebApplicationFactory();
        using var client = factory.CreateClient();

        const string password = "ValidPassword123!";

        // Register Participant A
        var emailA = $"profilea{Guid.NewGuid():N}@example.com";

        var registerA = await client.PostAsJsonAsync(
            "/api/Auth/register",
            new
            {
                FirstName = "Participant",
                Surname = "Alpha",
                Email = emailA,
                Password = password
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            registerA.StatusCode);

        // Register Participant B
        var emailB = $"profileb{Guid.NewGuid():N}@example.com";

        var registerB = await client.PostAsJsonAsync(
            "/api/Auth/register",
            new
            {
                FirstName = "Participant",
                Surname = "Beta",
                Email = emailB,
                Password = password
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            registerB.StatusCode);

        // Log in as Participant A
        var login = await client.PostAsJsonAsync(
            "/api/Auth/login",
            new { Email = emailA, Password = password });

        Assert.Equal(
            System.Net.HttpStatusCode.OK,
            login.StatusCode);

        var loginBody = await login.Content
            .ReadFromJsonAsync<LoginResponse>();

        Assert.NotNull(loginBody);

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer", loginBody.AccessToken);

        // Request the authenticated user's profile
        var profileResponse = await client.GetAsync("/api/Profile");

        Assert.Equal(
            System.Net.HttpStatusCode.OK,
            profileResponse.StatusCode);

        var profile = await profileResponse.Content
            .ReadFromJsonAsync<ProfileResponse>();

        Assert.NotNull(profile);

        // Verify Participant A's profile was returned
        Assert.Equal("Participant", profile.FirstName);
        Assert.Equal("Alpha", profile.Surname);
        Assert.Equal(emailA, profile.Email);

        // Ensure Participant B's email was not returned
        Assert.NotEqual(emailB, profile.Email);
    }
    private sealed class ProfileResponse
    {
        public string FirstName { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    [Fact]
    public async Task UnauthenticatedUser_CannotAccessProfile_ReturnsUnauthorized()
    {
        using var factory = new RaceDayWebApplicationFactory();
        using var client = factory.CreateClient();

        // Request the profile without logging in
        var response = await client.GetAsync("/api/Profile");

        Assert.Equal(
            System.Net.HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task Participant_CanUpdateOwnProfile_ReturnsOk()
    {
        using var factory = new RaceDayWebApplicationFactory();
        using var client = factory.CreateClient();

        var email = $"updateprofile{Guid.NewGuid():N}@example.com";
        const string password = "ValidPassword123!";

        // Register a Participant
        var registerResponse = await client.PostAsJsonAsync(
            "/api/Auth/register",
            new
            {
                FirstName = "Original",
                Surname = "Participant",
                Email = email,
                Password = password
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            registerResponse.StatusCode);

        // Log in
        var loginResponse = await client.PostAsJsonAsync(
            "/api/Auth/login",
            new { Email = email, Password = password });

        Assert.Equal(
            System.Net.HttpStatusCode.OK,
            loginResponse.StatusCode);

        var loginBody = await loginResponse.Content
            .ReadFromJsonAsync<LoginResponse>();

        Assert.NotNull(loginBody);

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer", loginBody.AccessToken);

        // Update the Participant's profile
        var updateResponse = await client.PutAsJsonAsync(
            "/api/Profile",
            new
            {
                FirstName = "Updated",
                Surname = "Runner",
                Email = email
            });

        Assert.Equal(
            System.Net.HttpStatusCode.OK,
            updateResponse.StatusCode);

        // Verify the updated details
        var profileResponse = await client.GetAsync("/api/Profile");

        Assert.Equal(
            System.Net.HttpStatusCode.OK,
            profileResponse.StatusCode);

        var profile = await profileResponse.Content
            .ReadFromJsonAsync<ProfileResponse>();

        Assert.NotNull(profile);
        Assert.Equal("Updated", profile.FirstName);
        Assert.Equal("Runner", profile.Surname);
        Assert.Equal(email, profile.Email);
    }

    [Fact]
    public async Task Participant_CannotUpdateProfileToExistingEmail_ReturnsConflict()
    {
        using var factory = new RaceDayWebApplicationFactory();
        using var client = factory.CreateClient();

        const string password = "ValidPassword123!";

        // Register Participant A
        var emailA = $"emailowner{Guid.NewGuid():N}@example.com";

        var registerA = await client.PostAsJsonAsync(
            "/api/Auth/register",
            new
            {
                FirstName = "Participant",
                Surname = "A",
                Email = emailA,
                Password = password
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            registerA.StatusCode);

        // Register Participant B
        var emailB = $"emailchanger{Guid.NewGuid():N}@example.com";

        var registerB = await client.PostAsJsonAsync(
            "/api/Auth/register",
            new
            {
                FirstName = "Participant",
                Surname = "B",
                Email = emailB,
                Password = password
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Created,
            registerB.StatusCode);

        // Log in as Participant B
        var login = await client.PostAsJsonAsync(
            "/api/Auth/login",
            new { Email = emailB, Password = password });

        Assert.Equal(
            System.Net.HttpStatusCode.OK,
            login.StatusCode);

        var loginBody = await login.Content
            .ReadFromJsonAsync<LoginResponse>();

        Assert.NotNull(loginBody);

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer", loginBody.AccessToken);

        // Attempt to change Participant B's email to Participant A's email
        var updateResponse = await client.PutAsJsonAsync(
            "/api/Profile",
            new
            {
                FirstName = "Participant",
                Surname = "B",
                Email = emailA
            });

        Assert.Equal(
            System.Net.HttpStatusCode.Conflict,
            updateResponse.StatusCode);
    }
}
