namespace SnakeGame.Contracts;

public interface IRandomGenerator
{
    int NextNumber(int min = 0, int max = int.MaxValue);
}