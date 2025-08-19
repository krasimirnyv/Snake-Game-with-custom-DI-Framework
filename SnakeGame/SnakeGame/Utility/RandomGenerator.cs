using SnakeGame.Contracts;

namespace SnakeGame.Utility;

public class RandomGenerator : IRandomGenerator
{
    private readonly Random _random;
    
    public RandomGenerator()
    {
        _random = new Random();
    }

    public int NextNumber(int min = 0, int max = int.MaxValue)
        => _random.Next(min, max);
}