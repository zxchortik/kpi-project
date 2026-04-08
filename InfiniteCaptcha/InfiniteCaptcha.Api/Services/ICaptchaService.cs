using InfiniteCaptcha.Shared.Models;

namespace InfiniteCaptcha.Api.Services
{
    public interface ICaptchaService
    {
        CaptchaChallengeDto GenerateChallenge(int level);

        bool VerifyAnswer(Guid challengeId, string answer);
    }
}