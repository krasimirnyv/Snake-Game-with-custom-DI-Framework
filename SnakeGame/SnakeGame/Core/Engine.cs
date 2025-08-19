using SnakeGame.Contracts;
using SnakeGame.Enums;
using SnakeGame.Models;
using SnakeGame.Utility;

namespace SnakeGame.Core;

public class Engine
{
     private const string UnknownFood = "Unknown food type!";
     
     private const int InitialGameSpeed = 150;
     
     private const int PlayAreaWidth = 140;
     private const int PlayAreaHeight = 31;
     
     private const int WallsWidth = 91;
     private const int WallsHeight = 31;
     
     private const int SnakeStartRight = 7;
     private const int SnakeStartUp = 5;
     private const int SnakeLength = 4;
     
     private const int FoodAsteriskPoints = 1;
     private const int FoodStarPoints = 1;
     private const int FoodSunPoints = 2;
     
     private readonly IRenderer _renderer;
     private readonly IGameInput _input;
     private readonly IRandomGenerator _random;

     private Snake _snake;
     private Food _food;
     private readonly HighScoreService _highScore;

     private int _gameSpeed;
     private int _playerScore;
     
     public Engine(IRenderer renderer, IGameInput input, IRandomGenerator random, IDateTimeProvider dateTime)
     {
          _renderer = renderer;
          _input = input;
          _random = random;
          _highScore = new HighScoreService(dateTime);
     }
     
     public void Run()
     {
          // 1. Initialize & draw the game - walls, snake, food, score, etc.
          InitializeGame();
          while (true)
          {
               Thread.Sleep(_gameSpeed);
               
               // 2. Move snake - right direction by default
               Point removedPoint = _snake.Move();
               
               // 3. Check key inputs and change direction
               Direction? inputDirection = _input.CheckForInput();
               if (inputDirection != null)
                    _snake.ChangeDirection(inputDirection.Value);
               
               // 4. Check collisions for game over - with wall or with self
               if (!ValidateSnakePositionIsValid())
               {
                    // 5. On game over, save high score
                    bool isUpdated = _highScore.UpdateIfHigher(_playerScore);
                    _renderer.RenderHighScore(_highScore.Load(), isUpdated);
                    _renderer.RenderGameOver();
                    break;
               }

               // 6. Check for food - increase score, snake length and game speed
               CheckForFood();
               _renderer.RenderSnake(_snake, removedPoint);
          }
          
          // 7. Ask player if they want to play again or exit
          if (_input.WaitForRestart()) Run();
          else Environment.Exit(0);
     }
     
     private void InitializeGame()
     {
          _playerScore = 0;
          _gameSpeed = InitialGameSpeed;
          _snake = new Snake(SnakeStartUp, SnakeStartRight, SnakeLength);
          
          _renderer.PrepareCanvas(PlayAreaWidth, PlayAreaHeight);
          _renderer.RenderWalls(WallsWidth, WallsHeight);
          _renderer.RenderSnake(_snake);
          _renderer.RenderScore(_playerScore);
          _renderer.RenderHighScore(_highScore.Load());
          
          GenerateNewFood();
     }

     private bool ValidateSnakePositionIsValid()
     {
          int countHeadPoints = 0;
          foreach (Point point in _snake.Body)
          {
               if (point.Right < 1 || point.Right >= WallsWidth - 1 ||
                   point.Up < 1 || point.Up >= WallsHeight - 1)
                    return false; // Snake hit the wall
               
               Point snakeHead = _snake.Head;
               if (point.Up == snakeHead.Up && point.Right == snakeHead.Right)
                    countHeadPoints++;
          }
          
          if (countHeadPoints > 1)
               return false; // Snake hit itself
          
          return true;
     }

     private void CheckForFood()
     {
          Point snakeHead = _snake.Head;
          
          if(_food.Right == snakeHead.Right &&
             _food.Up == snakeHead.Up)
          {
               if (_food is FoodAsterisk)
               {
                    _snake.IncreaseLength(FoodAsteriskPoints);
                    _playerScore += FoodAsteriskPoints;
               }
               else if (_food is FoodStar)
               {
                    _snake.IncreaseLength(FoodStarPoints);
                    _playerScore += FoodStarPoints;
               }
               else if (_food is FoodSun)
               {
                    _snake.IncreaseLength(FoodSunPoints);
                    _playerScore += FoodSunPoints;
               }
               else
                    throw new ArgumentException("Invalid food type!");
               
               _renderer.RenderScore(_playerScore);
               _gameSpeed -= 5;
               
               GenerateNewFood();
          }
     }
     
     private void GenerateNewFood()
     {
          while (true)
          {
               bool isValidFood = true;

               int foodRight = _random.NextNumber(2, WallsWidth - 2);
               int foodUp = _random.NextNumber(2, WallsHeight - 2);

               if (foodRight % 2 == 0)
                    foodRight++;

               foreach (Point point in _snake.Body)
               {
                    if(point.Up == foodUp && point.Right == foodRight)
                    {
                         isValidFood = false;
                         break;
                    }
               }

               if (isValidFood)
               {
                    int foodIndex = _random.NextNumber(0, 3);
                    switch (foodIndex)
                    {
                         case 0: _food = new FoodStar(foodUp, foodRight); break;
                         case 1: _food = new FoodSun(foodUp, foodRight); break;
                         case 2: _food = new FoodAsterisk(foodUp, foodRight); break;
                    }

                    break;
               }
          }
          
          _renderer.RenderFood(_food);
     }
}