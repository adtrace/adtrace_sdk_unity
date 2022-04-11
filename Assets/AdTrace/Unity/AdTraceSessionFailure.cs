//
//  Created by Nasser Amini (namini40@github.com) on April 2022.
//  Copyright (c) AdTrace (adtrace.io) . All rights reserved.

using System;
using System.Collections.Generic;

namespace io.adtrace.sdk
{
    public class AdTraceSessionFailure
    {
        public string Adid { get; set; }
        public string Message { get; set; }
        public string Timestamp { get; set; }
        public bool WillRetry { get; set; }
        public Dictionary<string, object> JsonResponse { get; set; }

        public AdTraceSessionFailure() {}

        public AdTraceSessionFailure(Dictionary<string, string> sessionFailureDataMap)
        {
            if (sessionFailureDataMap == null)
            {
                return;
            }

            Adid = AdTraceUtils.TryGetValue(sessionFailureDataMap, AdTraceUtils.KeyAdid);
            Message = AdTraceUtils.TryGetValue(sessionFailureDataMap, AdTraceUtils.KeyMessage);
            Timestamp = AdTraceUtils.TryGetValue(sessionFailureDataMap, AdTraceUtils.KeyTimestamp);

            bool willRetry;
            if (bool.TryParse(AdTraceUtils.TryGetValue(sessionFailureDataMap, AdTraceUtils.KeyWillRetry), out willRetry))
            {
                WillRetry = willRetry;
            }

            string jsonResponseString = AdTraceUtils.TryGetValue(sessionFailureDataMap, AdTraceUtils.KeyJsonResponse);
            var jsonResponseNode = JSON.Parse(jsonResponseString);
            if (jsonResponseNode != null && jsonResponseNode.AsObject != null)
            {
                JsonResponse = new Dictionary<string, object>();
                AdTraceUtils.WriteJsonResponseDictionary(jsonResponseNode.AsObject, JsonResponse);
            }
        }

        public AdTraceSessionFailure(string jsonString)
        {
            var jsonNode = JSON.Parse(jsonString);
            if (jsonNode == null) 
			{
                return;
            }

            Adid = AdTraceUtils.GetJsonString(jsonNode, AdTraceUtils.KeyAdid);
            Message = AdTraceUtils.GetJsonString(jsonNode, AdTraceUtils.KeyMessage);
            Timestamp = AdTraceUtils.GetJsonString(jsonNode, AdTraceUtils.KeyTimestamp);
            WillRetry = Convert.ToBoolean(AdTraceUtils.GetJsonString(jsonNode, AdTraceUtils.KeyWillRetry));

            var jsonResponseNode = jsonNode[AdTraceUtils.KeyJsonResponse];
            if (jsonResponseNode == null)
            {
                return;
            }
            if (jsonResponseNode.AsObject == null)
            {
                return;
            }

            JsonResponse = new Dictionary<string, object>();
            AdTraceUtils.WriteJsonResponseDictionary(jsonResponseNode.AsObject, JsonResponse);
        }

        public void BuildJsonResponseFromString(string jsonResponseString)
        {
            var jsonNode = JSON.Parse(jsonResponseString);
            if (jsonNode == null)
            {
                return;
            }

            JsonResponse = new Dictionary<string, object>();
            AdTraceUtils.WriteJsonResponseDictionary(jsonNode.AsObject, JsonResponse);
        }

        public string GetJsonResponse()
        {
            return AdTraceUtils.GetJsonResponseCompact(JsonResponse);
        }
    }
}
