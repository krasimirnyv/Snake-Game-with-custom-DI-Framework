namespace SnakeGame.Contracts;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}