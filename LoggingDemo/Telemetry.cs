using System.Diagnostics;

namespace LoggingDemo
{
    public static class Telemetry
    {
        public static ActivitySource ActivitySource = new ActivitySource("LoggingDemo");
    }
}
