namespace TektonChallengeProducts.Infrastructure.Services;

using System;
using System.IO;
using Application.Services;

public class LoggerService : ILoggerService
{
    private readonly string logFilePath;
    private readonly object lockObject = new();

    public LoggerService()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string logDirectory = Path.Combine(currentDirectory, "logs");
        logFilePath = Path.Combine(logDirectory, "requests.txt");

        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }
    }

    public void LogInformation(string message)
    {
        string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

        string logEntry = $"[{timestamp}] [INFO] {message}";

        lock (lockObject)
        {
            try
            {
                using StreamWriter writer = new(logFilePath, true);
                writer.WriteLine(logEntry);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al escribir en el archivo de log: " + ex.Message);
            }
        }
    }
}
