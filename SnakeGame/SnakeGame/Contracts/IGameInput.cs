using SnakeGame.Enums;

namespace SnakeGame.Contracts;

public interface IGameInput
{
    Direction? CheckForInput();

    bool WaitForRestart();
}