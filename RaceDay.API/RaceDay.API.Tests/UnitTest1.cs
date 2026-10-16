using System.ComponentModel.DataAnnotations;
using RaceDay.API.Models.DTOs;

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
}
