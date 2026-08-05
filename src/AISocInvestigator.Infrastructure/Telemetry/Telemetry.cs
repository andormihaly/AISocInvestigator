using System.Diagnostics;

namespace AISocInvestigator.Infrastructure.Telemetry;

public static class Telemetry
{
    public static readonly ActivitySource ActivitySource = new(TelemetryConstants.ActivitySourceName);
}