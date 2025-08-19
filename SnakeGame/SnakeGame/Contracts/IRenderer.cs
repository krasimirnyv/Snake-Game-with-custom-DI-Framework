using SnakeGame.Models;

namespace SnakeGame.Contracts;

public interface IRenderer
{
    void PrepareCanvas(int maxWidth, int maxHeight);
    
    void RenderWalls(int maxWidth, int maxHeight);

    void RenderSnake(Snake snake, Point? toRemove = null);
    
    void RenderFood(Food food);
    
    void RenderScore(int playerScore);

    void RenderHighScore(int score, bool isUpdated = false);

    void RenderGameOver();
}