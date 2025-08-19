namespace SnakeGame.Utility;

internal class HighScoreModel
{
    public HighScoreModel(int value, DateTime updatedAt)
    {
        Value = value;
        UpdatedAt = updatedAt;
    }

    public int Value { get; init; }
    public DateTime UpdatedAt { get; init; }
}