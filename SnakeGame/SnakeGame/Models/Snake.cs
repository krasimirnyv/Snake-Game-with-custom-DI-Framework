using SnakeGame.Enums;

namespace SnakeGame.Models;

public class Snake
{
    private const string ExceptionMessage = "Invalid direction {0}";
    
    public Snake(int startUp, int startRight, int lenght)
    {
        Body = new Queue<Point>(lenght);
        Head = new Point(startUp, startRight);
        Direction = Direction.Right;
        
        for (int i = startRight - lenght + 1; i < startRight; i++)
        {
            Point tailPoint = new Point(startUp, i);
            Body.Enqueue(tailPoint);
        }
        
        Body.Enqueue(Head);
    }
    
    public Point Head { get; private set; }
    public Queue<Point> Body { get; }
    public Direction Direction { get; private set; }

    public Point Move()
    {
        Point removedPart = Body.Dequeue();
        
        Point newHead = SetNewHead();
        Body.Enqueue(newHead);
        
        return removedPart;
    }

    public void ChangeDirection(Direction direction)
    {
        if (Direction == Direction.Right && direction == Direction.Left ||
            Direction == Direction.Left && direction == Direction.Right ||
            Direction == Direction.Up && direction == Direction.Down ||
            Direction == Direction.Down && direction == Direction.Up)
        {
            // Invalid direction change, do nothing
            return;
        }
        
        Direction = direction;
    }

    public void IncreaseLength(int length)
    {
        for (int i = 0; i < length; i++)
        {
            Point newHead = SetNewHead();
            Body.Enqueue(newHead);
        }
    }
    
    private Point SetNewHead()
    {
        Point directionPoint = GetDirectionPoint();
        
        int newHeadUp = Head.Up + directionPoint.Up;
        int newHeadRight = Head.Right + directionPoint.Right;
        
        Head = new Point(newHeadUp, newHeadRight);
        return Head;
    }
    
    private Point GetDirectionPoint()
    {
        if (Direction == Direction.Right)
        {
            return new Point(0, 2);
        }
        else if (Direction == Direction.Left)
        {
            return new Point(0, -2);
        }
        else if (Direction == Direction.Up)
        {
            return new Point(-1, 0);
        }
        else if (Direction == Direction.Down)
        {
            return new Point(1, 0);
        }
        
        throw new InvalidOperationException(string.Format(ExceptionMessage, Direction));
    }
}