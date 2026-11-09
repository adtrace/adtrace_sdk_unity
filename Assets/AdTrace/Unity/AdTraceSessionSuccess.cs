using System;
using System.Collections.Generic;

namespace io.adtrace.sdk
{
    public class AdTraceSessionSuccess
    {
        public string Adid { get; set; }
        public string Message { get; set; }
        public string Timestamp { get; set; }
        public Dictionary<string, object> JsonResponse { get; set; }

        public AdTraceSessionSuccess() {}

        public AdTraceSessionSuccess(Dictionary<string, string> sessionSuccessDataMap)
        {
            if (sessionSuccessDataMap == null)
            {
                return;
            }

            Adid = AdTraceUtils.TryGetValue(sessionSuccessDataMap, AdTraceUtils.KeyAdid);
            Message = AdTraceUtils.TryGetValue(sessionSuccessDataMap, AdTraceUtils.KeyMessage);
            Timestamp = AdTraceUtils.TryGetValue(sessionSuccessDataMap, AdTraceUtils.KeyTimestamp);

            string jsonResponseString = AdTraceUtils.TryGetValue(sessionSuccessDataMap, AdTraceUtils.KeyJsonResponse);
            var jsonResponseNode = JSON.Parse(jsonResponseString);
            if (jsonResponseNode != null && jsonResponseNode.AsObject != null)
            {
                JsonResponse = new Dictionary<string, object>();
                AdTraceUtils.WriteJsonResponseDictionary(jsonResponseNode.AsObject, JsonResponse);
            }
        }

        public AdTraceSessionSuccess(string jsonString)
        {
            var jsonNode = JSON.Parse(jsonString);
            if (jsonNode == null) 
			{
                return;
            }

            Adid = AdTraceUtils.GetJsonString(jsonNode, AdTraceUtils.KeyAdid);
            Message = AdTraceUtils.GetJsonString(jsonNode, AdTraceUtils.KeyMessage);
            Timestamp = AdTraceUtils.GetJsonString(jsonNode, AdTraceUtils.KeyTimestamp);

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
