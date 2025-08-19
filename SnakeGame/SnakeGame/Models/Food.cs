namespace SnakeGame.Models;

public abstract class Food : Point
{
    public Food(int up, int right) 
        : base(up, right)
    {
    }
}