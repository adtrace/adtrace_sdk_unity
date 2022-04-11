//
//  Created by Nasser Amini (namini40@github.com) on April 2022.
//  Copyright (c) AdTrace (adtrace.io) . All rights reserved.

using System;
using System.Collections.Generic;

namespace io.adtrace.sdk
{
    public class AdTraceAttribution
    {
        public string adid { get; set; }
        public string network { get; set; }
        public string adgroup { get; set; }
        public string campaign { get; set; }
        public string creative { get; set; }
        public string clickLabel { get; set; }
        public string trackerName { get; set; }
        public string trackerToken { get; set; }
        public string costType { get; set; }
        public double? costAmount { get; set; }
        public string costCurrency { get; set; }

        public AdTraceAttribution() {}

        public AdTraceAttribution(string jsonString)
        {
            var jsonNode = JSON.Parse(jsonString);
            if (jsonNode == null)
            {
                return;
            }

            trackerName = AdTraceUtils.GetJsonString(jsonNode, AdTraceUtils.KeyTrackerName);
            trackerToken = AdTraceUtils.GetJsonString(jsonNode, AdTraceUtils.KeyTrackerToken);
            network = AdTraceUtils.GetJsonString(jsonNode, AdTraceUtils.KeyNetwork);
            campaign = AdTraceUtils.GetJsonString(jsonNode, AdTraceUtils.KeyCampaign);
            adgroup = AdTraceUtils.GetJsonString(jsonNode, AdTraceUtils.KeyAdgroup);
            creative = AdTraceUtils.GetJsonString(jsonNode, AdTraceUtils.KeyCreative);
            clickLabel = AdTraceUtils.GetJsonString(jsonNode, AdTraceUtils.KeyClickLabel);
            adid = AdTraceUtils.GetJsonString(jsonNode, AdTraceUtils.KeyAdid);
            costType = AdTraceUtils.GetJsonString(jsonNode, AdTraceUtils.KeyCostType);
            try
            {
                costAmount = double.Parse(AdTraceUtils.GetJsonString(jsonNode, AdTraceUtils.KeyCostAmount),
                System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                // attribution response doesn't contain cost amount attached
                // value will default to null
            }
            costCurrency = AdTraceUtils.GetJsonString(jsonNode, AdTraceUtils.KeyCostCurrency);
        }

        public AdTraceAttribution(Dictionary<string, string> dicAttributionData)
        {
            if (dicAttributionData == null)
            {
                return;
            }

            trackerName = AdTraceUtils.TryGetValue(dicAttributionData, AdTraceUtils.KeyTrackerName);
            trackerToken = AdTraceUtils.TryGetValue(dicAttributionData, AdTraceUtils.KeyTrackerToken);
            network = AdTraceUtils.TryGetValue(dicAttributionData, AdTraceUtils.KeyNetwork);
            campaign = AdTraceUtils.TryGetValue(dicAttributionData, AdTraceUtils.KeyCampaign);
            adgroup = AdTraceUtils.TryGetValue(dicAttributionData, AdTraceUtils.KeyAdgroup);
            creative = AdTraceUtils.TryGetValue(dicAttributionData, AdTraceUtils.KeyCreative);
            clickLabel = AdTraceUtils.TryGetValue(dicAttributionData, AdTraceUtils.KeyClickLabel);
            adid = AdTraceUtils.TryGetValue(dicAttributionData, AdTraceUtils.KeyAdid);
            costType = AdTraceUtils.TryGetValue(dicAttributionData, AdTraceUtils.KeyCostType);
            try
            {
                costAmount = double.Parse(AdTraceUtils.TryGetValue(dicAttributionData, AdTraceUtils.KeyCostAmount),
                System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                // attribution response doesn't contain cost amount attached
                // value will default to null
            }
            costCurrency = AdTraceUtils.TryGetValue(dicAttributionData, AdTraceUtils.KeyCostCurrency);
        }
    }
}
