namespace InfiniteCaptcha.Shared.Models;

public class CaptchaAttemptDto
{
    public Guid ChallengeId { get; set; }
    public string Answer { get; set; } = string.Empty;
}