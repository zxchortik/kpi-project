namespace InfiniteCaptcha.Shared.Models;

public class CaptchaResultDto
{
    public bool IsSuccess { get; set; }
    public int CurrentScore { get; set; }
    public CaptchaChallengeDto? NextChallenge { get; set; }
}