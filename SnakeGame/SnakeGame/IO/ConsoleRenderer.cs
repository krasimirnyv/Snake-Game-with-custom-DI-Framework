using SnakeGame.Contracts;
using SnakeGame.Models;

namespace SnakeGame.IO;

public class ConsoleRenderer : IRenderer
{
    private const string UnknownFood = "Unknown food type!";
    
    private int _maxAreaWidth;
    private int _maxAreaHeight;
    
    private int _maxWallWidth;
    private int _maxWallHeight;
    
    private const char HorizontalWall = '─';
    private const char VerticalWall = '│';
    
    private const char SnakeHeadSymbol = '■';
    private const char SnakeBodySymbol = '●';
    private const char SnakeTailSymbol = '-';
    
    private const char FoodAsteriskSymbol = '¤';
    private const char FoodStarSymbol = '★';
    private const char FoodSunSymbol = '☀';
    
    private const char EmptySymbol = ' ';
    
    public void PrepareCanvas(int maxWidth, int maxHeight)
    {
        _maxAreaWidth = maxWidth;
        _maxAreaHeight = maxHeight;
        
        Console.Clear();
        Console.CursorVisible = false;
        
        if (OperatingSystem.IsWindows())
            Console.SetWindowSize(maxWidth, maxHeight);
        else
            Console.Write($"\u001b[8;{maxHeight};{maxWidth}t");
    }

    public void RenderWalls(int maxWidth, int maxHeight)
    {
        _maxWallWidth = maxWidth;
        _maxWallHeight = maxHeight;
        
        for (int i = 0; i < maxWidth; i++) // Top wall
        {
            Console.SetCursorPosition(i, 0);
            Console.Write(HorizontalWall);
        }
        
        for (int i = 0; i < maxWidth; i++) // Bottom wall
        {
            Console.SetCursorPosition(i, maxHeight);
            Console.Write(HorizontalWall);
        }
        
        for (int i = 0; i < maxHeight; i++) // Left wall
        {
            Console.SetCursorPosition(0, i);
            Console.Write(VerticalWall);
        }
        
        for (int i = 0; i < maxHeight; i++) // Right wall
        {
            Console.SetCursorPosition(maxWidth, i);
            Console.Write(VerticalWall);
        }
    }

    public void RenderSnake(Snake snake, Point? toRemove = null)
    {
        if (toRemove != null)
        {
            Console.SetCursorPosition(toRemove.Right, toRemove.Up);
            Console.Write(EmptySymbol);
        }
        
        foreach (Point point in snake.Body)
        {
            Console.SetCursorPosition(point.Right, point.Up);
            if (point == snake.Head)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(SnakeHeadSymbol);
                Console.ResetColor();
                continue;
            }

            if (point == snake.Body.First())
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(SnakeTailSymbol);
                Console.ResetColor();
                continue;
            }
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(SnakeBodySymbol);
            Console.ResetColor();
        }
    }
    
    public void RenderFood(Food food)
    {
        Console.SetCursorPosition(food.Right, food.Up);
        Console.ForegroundColor = ConsoleColor.Red;
        
        if (food is FoodAsterisk)
            Console.Write(FoodAsteriskSymbol);
        else if (food is FoodStar)
            Console.Write(FoodStarSymbol);
        else if (food is FoodSun)
            Console.Write(FoodSunSymbol);
        else
            throw new ArgumentException(UnknownFood);
        
        Console.ResetColor();
    }

    public void RenderScore(int playerScore)
    {
        Console.SetCursorPosition(_maxWallWidth + 5, 3);
        Console.Write($"Score: {playerScore}");
    }

    public void RenderHighScore(int score, bool isUpdated = false)
    {
        Console.SetCursorPosition(_maxWallWidth + 5, 2);
        if (isUpdated)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"Your personal best: {score}!");
            Console.ResetColor();
        }
        else
            Console.Write($"Your High Score: {score}");
    }

    public void RenderGameOver()
    {
        Console.SetCursorPosition(_maxWallWidth / 2 - 6, _maxWallHeight / 2 - 2);
        Console.Write("GAME OVER!");
        
        Console.SetCursorPosition(_maxWallWidth / 2 - 13, _maxWallHeight / 2);
        Console.Write("Press \"ENTER\" to Restart...");
        
        Console.SetCursorPosition(_maxWallWidth + 4, _maxAreaHeight - 2);
        Console.Write("Press \"SPACE\" to Stop");
    }
}