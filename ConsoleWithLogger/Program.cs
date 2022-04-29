// See https://aka.ms/new-console-template for more information
using Serilog;
using Serilog.Core;
using Serilog.Formatting.Compact;

public class Program
{
    private static Logger _logger;

    public static void Main()
    {
        using (_logger = CreateLogger())
        {
            ProcessUsingInterpolation("Tom");
            ProcessUsingStructuredLogging("Ben");
        }
    }

    public static Logger CreateLogger()
    {
        return new LoggerConfiguration()
        .WriteTo.Console()
        .WriteTo.File(new CompactJsonFormatter(), "logs.txt")
        .WriteTo.Seq("http://localhost:5341/")
        .CreateLogger();
    }

    public static void ProcessUsingInterpolation(string userName)
    {
        _logger.Information($"User {userName} registered at {DateTime.UtcNow}");

        Guid orderId = Guid.NewGuid();
        _logger.Information($"User {userName} created new order {orderId}", userName, orderId);
        _logger.Information($"Order {orderId} shipped", orderId);
        _logger.Information($"Order {orderId} finished", orderId);

        Guid secondOrderId = Guid.NewGuid();
        _logger.Information($"User {userName} created new order {secondOrderId}");
        _logger.Information($"Order {secondOrderId} canceled");

    }

    public static void ProcessUsingStructuredLogging(string userName)
    {
        _logger.Information("User {userName} registered at {registeredDate}", userName, DateTime.UtcNow);

        Guid orderId = Guid.NewGuid();
        _logger.Information("User {userName} created new order {orderId}", userName, orderId);
        _logger.Information("Order {orderId} shipped", orderId);
        _logger.Information("Order {orderId} finished", orderId);

        Guid secondOrderId = Guid.NewGuid();
        _logger.Information("User {userName} created new order {orderId}", userName, secondOrderId);
        _logger.Information("Order {orderId} canceled", secondOrderId);
    }

}
