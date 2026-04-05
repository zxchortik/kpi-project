namespace InfiniteCaptcha.Shared.Models;

public class CaptchaChallengeDto
{
    public Guid Id { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string? ImageBase64 { get; set; }
    public int DifficultyLevel { get; set; }
}