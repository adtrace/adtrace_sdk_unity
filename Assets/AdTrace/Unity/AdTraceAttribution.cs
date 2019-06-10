using System;
using System.Collections.Generic;

namespace com.adtrace.sdk
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
        }
    }
}
