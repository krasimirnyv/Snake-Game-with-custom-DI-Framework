using System.Text.Json;
using SnakeGame.Contracts;

namespace SnakeGame.Utility;

public sealed class HighScoreService
{
    private readonly IDateTimeProvider _dateTime;
    private readonly string _appDir;
    private readonly string _filePath;
    private readonly object _sync = new();
    private int? _cached;
    
    public HighScoreService(IDateTimeProvider dateTime, string? applicationDirectory = null, string fileName = "highScore.json")
    {
        _dateTime = dateTime;
        
       _appDir = applicationDirectory ?? Path.Combine(
           Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SnakeGame");

       _filePath = Path.Combine(_appDir, fileName);
    }

    public string PrintFilePath()
        => $"HighScore file is located at: {_filePath}";
    
    internal bool UpdateIfHigher(int score)
    {
        int current = Load();
        return score > current && Save(score);
    }

    internal int Load()
    {
        if (_cached.HasValue)
            return _cached.Value;
        
        try
        {
            if (!File.Exists(_filePath))
                return (_cached = 0).Value;

            string json = File.ReadAllText(_filePath);
            HighScoreModel? model = JsonSerializer.Deserialize<HighScoreModel>(json);
            return (_cached = (model?.Value ?? 0)).Value;
        }
        catch
        {
            return (_cached = 0).Value;
        }
    }

    private bool Save(int value)
    {
        if(value < 0)
            return false;

        string? temp = null;
        
        lock (_sync)
        {
            try
            {
                Directory.CreateDirectory(_appDir);

                HighScoreModel? model = new HighScoreModel(value, _dateTime.UtcNow);
                string json = JsonSerializer.Serialize(model, new JsonSerializerOptions { WriteIndented = true });

                temp = _filePath + ".tmp";
                File.WriteAllText(temp, json);

                try
                {
                    if (File.Exists(_filePath))
                        File.Replace(temp, _filePath, null);
                    else
                        File.Move(temp, _filePath);
                }
                catch (IOException)
                {
                    File.Move(temp, _filePath, overwrite: true);
                }

                _cached = value;
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                // Handle unauthorized access, e.g., if the file is read-only or locked by another process
                return false;
            }
            catch (IOException)
            {
                // Handle IO exceptions, such as file being in use or disk issues
                return false;
            }
            catch (JsonException)
            {
                // Handle JSON serialization/deserialization errors
                return false;
            }
            finally
            {
                if (temp is not null && File.Exists(temp))
                {
                    try
                    {
                        File.Delete(temp);
                    }
                    catch
                    {
                        // Ignore any exceptions during cleanup
                    }
                }
            }
        }
    }
}