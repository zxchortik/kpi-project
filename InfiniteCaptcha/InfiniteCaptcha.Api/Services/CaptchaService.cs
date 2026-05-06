using InfiniteCaptcha.Shared.Models;
using System.Collections.Concurrent;

namespace InfiniteCaptcha.Api.Services
{
    public class CaptchaService : ICaptchaService
    {
        private static readonly ConcurrentDictionary<Guid, string> _answers = new();
        private readonly Random _random = new();

        public CaptchaChallengeDto GenerateChallenge(int level)
        {
            var challengeId = Guid.NewGuid();
            string question = "";
            string correctAnswer = "";

            int maxCategory = Math.Min(6, (level / 3) + 1);
            int category = _random.Next(1, maxCategory + 1);

            switch (category)
            {
                case 1:
                    int a = _random.Next(1, 10 + level * 2);
                    int b = _random.Next(1, 10 + level * 2);
                    if (_random.NextDouble() > 0.5)
                    {
                        question = $"{a} + {b} = ?";
                        correctAnswer = (a + b).ToString();
                    }
                    else
                    {
                        int max = Math.Max(a, b);
                        int min = Math.Min(a, b);
                        question = $"{max} - {min} = ?";
                        correctAnswer = (max - min).ToString();
                    }
                    break;

                case 2:
                    string[,] itQuestions = {
                        { "HTTP статус 'Not Found'?", "404" },
                        { "HTTP статус 'OK'?", "200" },
                        { "Скільки біт в 1 байті?", "8" },
                        { "Стандартний порт HTTP?", "80" },
                        { "Стандартний порт HTTPS?", "443" },
                        { "2 в 10 ступені?", "1024" },
                        { "Головна парадигма C# (три літери)?", "oop" },
                        { "Який фреймворк ми юзаємо для БД (.NET)?", "ef" }
                    };
                    int qIndex = _random.Next(itQuestions.GetLength(0));
                    question = itQuestions[qIndex, 0];
                    correctAnswer = itQuestions[qIndex, 1];
                    break;

                case 3:
                    string[] words = { "kpi", "fpm", "code", "bug", "hash", "git", "null", "push" };
                    string word = words[_random.Next(words.Length)];
                    question = $"Напиши задом наперед: {word}";

                    char[] charArray = word.ToCharArray();
                    Array.Reverse(charArray);
                    correctAnswer = new string(charArray).ToLower();
                    break;

                case 4:
                    int dec = _random.Next(5, 20 + level);
                    if (_random.NextDouble() > 0.5)
                    {
                        question = $"Переведи з BIN (двійкової) у DEC: {Convert.ToString(dec, 2)}";
                        correctAnswer = dec.ToString();
                    }
                    else
                    {
                        question = $"Переведи з HEX (шістнадцяткової) у DEC: {Convert.ToString(dec, 16).ToUpper()}";
                        correctAnswer = dec.ToString();
                    }
                    break;

                case 5:
                    int m11 = _random.Next(1, 5), m12 = _random.Next(1, 5);
                    int m21 = _random.Next(1, 5), m22 = _random.Next(1, 5);
                    if (_random.NextDouble() > 0.5)
                    {
                        question = $"Слід матриці (Trace) [[{m11}, {m12}], [{m21}, {m22}]] = ?";
                        correctAnswer = (m11 + m22).ToString();
                    }
                    else
                    {
                        question = $"Визначник (Det) [[{m11}, {m12}], [{m21}, {m22}]] = ?";
                        correctAnswer = ((m11 * m22) - (m12 * m21)).ToString();
                    }
                    break;

                case 6:
                    int coef = _random.Next(2, 6);
                    int power = _random.Next(2, 5);
                    if (_random.NextDouble() > 0.5)
                    {
                        question = $"f'(1) для f(x) = {coef}x^{power} ?";
                        correctAnswer = (coef * power).ToString();
                    }
                    else
                    {
                        int evenCoef = _random.Next(1, 5) * 2;
                        question = $"Визначений інтеграл від 0 до 1 для f(x) = {evenCoef}x dx ?";
                        correctAnswer = (evenCoef / 2).ToString();
                    }
                    break;

                default:
                    question = "1 + 1 = ?";
                    correctAnswer = "2";
                    break;
            }

            _answers[challengeId] = correctAnswer.ToLower();

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
                return correct == answer.Trim().ToLower();
            }
            return false;
        }
    }
}