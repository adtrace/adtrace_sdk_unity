namespace io.adtrace.sdk
{
    [System.Serializable]
    public enum AdTraceUrlStrategy
    {
        Default,
        DataResidencyEU,
        DataResidencyTK,
        DataResidencyUS,
        India,
        China,
    }

    public static class AdTraceUrlStrategyExtension
    {
        public static string ToLowerCaseString(this AdTraceUrlStrategy strategy)
        {
            switch (strategy)
            {
                case AdTraceUrlStrategy.India: return "india";
                case AdTraceUrlStrategy.China: return "china";
                case AdTraceUrlStrategy.DataResidencyEU: return "data-residency-eu";
                case AdTraceUrlStrategy.DataResidencyTK: return "data-residency-tr";
                case AdTraceUrlStrategy.DataResidencyUS: return "data-residency-us";
                default: return string.Empty;
            }
        }
    }
}

