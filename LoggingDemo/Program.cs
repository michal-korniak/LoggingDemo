using ConsoleWithLogger;
using LoggingDemo;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

public class Program
{
    public static async Task Main()
    {
        using (var loggerFactory = new AppLoggerFactory())
        {
            using (TracerProviderFactory.CreateTracerProvider(Telemetry.ActivitySource.Name))
            {
                var logger = loggerFactory.CreateLogger("TestLogger");
                await ProduceSomeLogs(logger, "Bob");
            }
        }
    }

    public static async Task ProduceSomeLogs(ILogger logger, string userName)
    {
        using (Telemetry.ActivitySource.StartActivity("MainActivity"))
        {
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
                using (Telemetry.ActivitySource.StartActivity("TryBlock"))
                {
                    await Task.Delay(500);
                    int y = 0;
                    int x = 100 / y;
                }
            }
            catch (Exception ex)
            {
                using (Telemetry.ActivitySource.StartActivity("CatchBlock"))
                {
                    ex.Data.Add("xx", "yy");
                    logger.LogError(new CustomException(ex), "Something's wrong.");
                }
                Activity.Current.SetStatus(ActivityStatusCode.Error, ex.Message);
            }
            logger.LogInformation("Processing finished");
        }
    }

}
