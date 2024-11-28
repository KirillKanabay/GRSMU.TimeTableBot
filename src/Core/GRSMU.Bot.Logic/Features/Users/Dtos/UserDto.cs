namespace GRSMU.Bot.Logic.Features.Users.Dtos;

public record UserDto
{
    public string Id { get; set; }

    public string TelegramId { get; set; }

    public long? ChatId { get; set; }

    public bool IsAdmin { get; set; }

    public string? TelegramFirstName { get; set; }

    public string? TelegramLastName { get; set; }

    public string? TelegramUsername { get; set; }

    public string? GroupId { get; set; }

    public string? CourseId { get; set; }

    public string? FacultyId { get; set; }

    public string? StudentCardLogin { get; set; }

    public string? StudentCardPassword { get; set; }

    public string? StudentFullName { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpireTime { get; set; }

    public bool IsStudentCardRegistered => 
        !string.IsNullOrWhiteSpace(StudentCardLogin) || 
        !string.IsNullOrWhiteSpace(StudentCardPassword) ||
        !string.IsNullOrWhiteSpace(GroupId) ||
        !string.IsNullOrWhiteSpace(CourseId) ||
        !string.IsNullOrWhiteSpace(FacultyId);
}