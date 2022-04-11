//
//  Created by Nasser Amini (namini40@github.com) on April 2022.
//  Copyright (c) AdTrace (adtrace.io) . All rights reserved.

namespace io.adtrace.sdk
{
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
