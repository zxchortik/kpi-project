namespace InfiniteCaptcha.Api.Data;

public class PlayerRecord
{
    public Guid Id { get; set; }
    public string PlayerName { get; set; } = string.Empty;
    public int HighestLevel { get; set; }
    public DateTime AchievedAt { get; set; }
}