namespace SnakeGame.Models;

public class Point
{
  public Point(int up, int right)
  {
    Up = up;
    Right = right;
  }
  
  public  int Up { get; private set; }
  public int Right { get; private set; }

  public override string ToString()
    => $"({Up}, {Right})";
}