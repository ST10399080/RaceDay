namespace RaceDay.MVC.Models;

public class LoginResponse
{
    public string Message { get; set; } = string.Empty;

    public string AccessToken { get; set; } = string.Empty;

    public string TokenType { get; set; } = string.Empty;

    public int ExpiresIn { get; set; }

    public LoginUserResponse? User { get; set; }
}

public class LoginUserResponse
{
    public int UserID { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string Surname { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;
}
