using ConsoleWithLogger;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using System.Diagnostics;

public class Program
{
    public static void Main()
    {
        using (var loggerFactory = new AppLoggerFactory())
        {
            ProduceSomeLogs(loggerFactory.CreateLogger("StructuredLogs"), "Ben");
            loggerFactory.CreateLogger("Main").LogInformation("In main");
        }
    }

    public static void ProduceSomeLogs(ILogger logger, string userName)
    {
        using var mainActivity = new Activity("main-mainActivity");
        mainActivity.Start();

        logger.LogInformation("User {userName} registered at {registeredDate}", userName, DateTime.UtcNow);

        Guid orderId = Guid.NewGuid();
        logger.LogInformation("User {userName} created new order {orderId}", userName, orderId);
        logger.LogInformation("Order {orderId} shipped", orderId);
        logger.LogInformation("Order {orderId} finished", orderId);

        Guid secondOrderId = Guid.NewGuid();
        logger.LogInformation("User {userName} created new order {orderId}", userName, secondOrderId);
        logger.LogInformation("Order {orderId} canceled", secondOrderId);

        try
        {
            int y = 0;
            int x = 100 / y;
        }
        catch (Exception ex)
        {
            using var errorHandlingActivity = new Activity("error-handling");
            errorHandlingActivity.SetParentId(Activity.Current.Id);
            errorHandlingActivity.Start();
            ex.Data.Add("xx", "yy");
            logger.LogError(new ArgumentException("eee", ex), "Something's wrong.");
        }
        logger.LogInformation("Processing finished");
    }

}
