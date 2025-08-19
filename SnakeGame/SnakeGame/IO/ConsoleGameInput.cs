using SnakeGame.Contracts;
using SnakeGame.Enums;

namespace SnakeGame.IO;

public class ConsoleGameInput : IGameInput
{
    public Direction? CheckForInput()
    {
        if (!Console.KeyAvailable)
            return null;
        
        ConsoleKeyInfo keyInfo = Console.ReadKey();
        
        if (keyInfo.Key == ConsoleKey.UpArrow)
        {
            return Direction.Up;
        }
        else if (keyInfo.Key == ConsoleKey.DownArrow)
        {
            return Direction.Down;
        }
        else if (keyInfo.Key == ConsoleKey.LeftArrow)
        {
            return Direction.Left;
        }
        else if (keyInfo.Key == ConsoleKey.RightArrow)
        {
            return Direction.Right;
        }
        
        return null;
    }

    public bool WaitForRestart()
    {
        while (true)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            
            if (keyInfo.Key == ConsoleKey.Enter) 
                return true;
            else if (keyInfo.Key == ConsoleKey.Spacebar)
                return false;
        }
    }
}