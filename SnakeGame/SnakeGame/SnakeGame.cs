using EasyInjector;
using SnakeGame.Contracts;
using SnakeGame.Core;
using SnakeGame.IO;
using SnakeGame.Utility;

namespace SnakeGame;

public class SnakeGame
{
    public static void Main()
    {
        Injector
            .Register<IRenderer, ConsoleRenderer>()
            .Register<IGameInput, ConsoleGameInput>()
            .Register<IRandomGenerator, RandomGenerator>()
            .Register<IDateTimeProvider, SystemDateTimeProvider>()
            .Create<Engine>()
            .Run();
    }
}