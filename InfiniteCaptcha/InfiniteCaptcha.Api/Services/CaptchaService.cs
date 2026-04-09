using InfiniteCaptcha.Shared.Models;
using System.Collections.Concurrent;

namespace InfiniteCaptcha.Api.Services
{
    public class CaptchaService : ICaptchaService
    {
        private static readonly ConcurrentDictionary<Guid, string> _answers = new();

        public CaptchaChallengeDto GenerateChallenge(int level)
        {
            var random = new Random();
            var challengeId = Guid.NewGuid();
            string question;
            int correctAnswer;

            if (level <= 5)
            {
                int a = random.Next(1, 10);
                int b = random.Next(1, 10);
                question = $"{a} + {b} = ?";
                correctAnswer = a + b;
            }
            else
            {
                int a = random.Next(10, 50);
                int b = random.Next(2, 10);
                int c = random.Next(1, 10);
                question = $"{a} + {b} * {c} = ?";
                correctAnswer = a + (b * c);
            }

            _answers[challengeId] = correctAnswer.ToString();

            return new CaptchaChallengeDto
            {
                Id = challengeId,
                QuestionText = question,
                DifficultyLevel = level
            };
        }

        public bool VerifyAnswer(Guid challengeId, string answer)
        {
            if (_answers.TryRemove(challengeId, out var correct))
            {
                return correct == answer.Trim();
            }
            return false;
        }
    }
}