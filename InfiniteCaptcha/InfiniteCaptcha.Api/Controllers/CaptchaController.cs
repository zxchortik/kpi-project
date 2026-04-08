using Microsoft.AspNetCore.Mvc;
using InfiniteCaptcha.Api.Services;
using InfiniteCaptcha.Shared.Models;

namespace InfiniteCaptcha.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class CaptchaController : ControllerBase
    {
        private readonly ICaptchaService _captchaService;

        public CaptchaController(ICaptchaService captchaService)
        {
            _captchaService = captchaService;
        }

        [HttpGet("generate/{level}")]
        public ActionResult<CaptchaChallengeDto> GetChallenge(int level)
        {
            var challenge = _captchaService.GenerateChallenge(level);
            return Ok(challenge);
        }

        [HttpPost("verify")]
        public ActionResult<CaptchaResultDto> Verify([FromBody] CaptchaAttemptDto attempt)
        {
            bool isCorrect = _captchaService.VerifyAnswer(attempt.ChallengeId, attempt.Answer);

            var result = new CaptchaResultDto
            {
                IsSuccess = isCorrect,
                CurrentScore = isCorrect ? 1 : 0, 
                NextChallenge = isCorrect ? _captchaService.GenerateChallenge(2) : null
            };

            return Ok(result);
        }
    }
}