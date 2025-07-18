namespace TektonChallengeProducts.Infrastructure.Services;

using System;
using System.IO;
using Application.Services;

public class LoggerService : ILoggerService
{
    private const string logFilePath= "logs/requests.txt";

    public LoggerService()
    {
        if (!File.Exists(logFilePath))
        {
            File.Create(logFilePath).Close();
        }
    }

    public void LogInformation(string message)
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        string logEntry = $"[{timestamp}] [INFO] {message}";

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
