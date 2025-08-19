using SnakeGame.Contracts;

namespace SnakeGame.Utility;

public class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}