namespace io.adtrace.sdk
{
    [System.Serializable]
    public enum AdTraceLogLevel
    {
        Verbose = 1,
        Debug,
        Info,
        Warn,
        Error,
        Assert,
        Suppress
    }

    public static class AdTraceLogLevelExtension
    {
        public static string ToLowercaseString(this AdTraceLogLevel AdTraceLogLevel)
        {
            switch (AdTraceLogLevel)
            {
                case AdTraceLogLevel.Verbose:
                    return "verbose";
                case AdTraceLogLevel.Debug:
                    return "debug";
                case AdTraceLogLevel.Info:
                    return "info";
                case AdTraceLogLevel.Warn:
                    return "warn";
                case AdTraceLogLevel.Error:
                    return "error";
                case AdTraceLogLevel.Assert:
                    return "assert";
                case AdTraceLogLevel.Suppress:
                    return "suppress";
                default:
                    return "unknown";
            }
        }

        public static string ToUppercaseString(this AdTraceLogLevel AdTraceLogLevel)
        {
            switch (AdTraceLogLevel)
            {
                case AdTraceLogLevel.Verbose:
                    return "VERBOSE";
                case AdTraceLogLevel.Debug:
                    return "DEBUG";
                case AdTraceLogLevel.Info:
                    return "INFO";
                case AdTraceLogLevel.Warn:
                    return "WARN";
                case AdTraceLogLevel.Error:
                    return "ERROR";
                case AdTraceLogLevel.Assert:
                    return "ASSERT";
                case AdTraceLogLevel.Suppress:
                    return "SUPPRESS";
                default:
                    return "UNKNOWN";
            }
        }
    }
}
