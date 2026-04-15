using Microsoft.AspNetCore.Mvc;
using InfiniteCaptcha.Api.Services;
using InfiniteCaptcha.Shared.Models;
using InfiniteCaptcha.Api.Data;

namespace InfiniteCaptcha.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CaptchaController : ControllerBase
    {
        private readonly ICaptchaService _captchaService;
        private readonly AppDbContext _context;

        public CaptchaController(ICaptchaService captchaService, AppDbContext context)
        {
            _captchaService = captchaService;
            _context = context;
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

            if (!isCorrect)
            {
                var newRecord = new PlayerRecord
                {
                    Id = Guid.NewGuid(),
                    PlayerName = string.IsNullOrWhiteSpace(attempt.PlayerName) ? "Anonimus" : attempt.PlayerName,
                    HighestLevel = attempt.CurrentLevel - 1, 
                    AchievedAt = DateTime.UtcNow
                };

                _context.PlayerRecords.Add(newRecord);
                _context.SaveChanges();
            }

            var result = new CaptchaResultDto
            {
                IsSuccess = isCorrect,
                CurrentScore = isCorrect ? attempt.CurrentLevel : 0,
                NextChallenge = isCorrect ? _captchaService.GenerateChallenge(attempt.CurrentLevel + 1) : null
            };

            return Ok(result);
        }

        [HttpGet("leaderboard")]
        public ActionResult<IEnumerable<PlayerRecordDto>> GetLeaderboard()
        {
            var topPlayers = _context.PlayerRecords
                .OrderByDescending(r => r.HighestLevel) 
                .ThenBy(r => r.AchievedAt) 
                .Select(r => new PlayerRecordDto
                {
                    PlayerName = r.PlayerName,
                    HighestLevel = r.HighestLevel,
                    AchievedAt = r.AchievedAt
                })
                .ToList();

            return Ok(topPlayers);
        }
    }
}