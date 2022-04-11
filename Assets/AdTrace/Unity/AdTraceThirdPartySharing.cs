//
//  Created by Nasser Amini (namini40@github.com) on April 2022.
//  Copyright (c) AdTrace (adtrace.io) . All rights reserved.

using System;
using System.Collections.Generic;

namespace io.adtrace.sdk
{
    public class AdTraceThirdPartySharing
    {
        internal bool? isEnabled;
        internal Dictionary<string, List<string>> granularOptions;

        public AdTraceThirdPartySharing(bool? isEnabled)
        {
            this.isEnabled = isEnabled;
            this.granularOptions = new Dictionary<string, List<string>>();
        }

        public void addGranularOption(string partnerName, string key, string value)
        {
            // TODO: consider to add some logs about the error case
            if (partnerName == null || key == null || value == null)
            {
                return;
            }

            List<string> partnerOptions;
            if (granularOptions.ContainsKey(partnerName))
            {
                partnerOptions = granularOptions[partnerName];
            }
            else
            {
                partnerOptions = new List<string>();
                granularOptions.Add(partnerName, partnerOptions);
            }

            partnerOptions.Add(key);
            partnerOptions.Add(value);
        }
    }
}
