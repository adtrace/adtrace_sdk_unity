namespace io.adtrace.sdk
{
    [System.Serializable]
    public enum AdTraceEnvironment
    {
        Sandbox,
        Production
    }

    public static class AdTraceEnvironmentExtension
    {
        public static string ToLowercaseString(this AdTraceEnvironment adtraceEnvironment)
        {
            switch (adtraceEnvironment)
            {
                case AdTraceEnvironment.Sandbox:
                    return "sandbox";
                case AdTraceEnvironment.Production:
                    return "production";
                default:
                    return "unknown";
            }
        }
    }
}
