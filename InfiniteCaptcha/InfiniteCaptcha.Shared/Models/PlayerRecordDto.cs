namespace InfiniteCaptcha.Shared.Models
{
    public class PlayerRecordDto
    {
        public string PlayerName { get; set; } = string.Empty;
        public int HighestLevel { get; set; }
        public DateTime AchievedAt { get; set; }
    }
}