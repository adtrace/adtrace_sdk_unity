using System;
using System.Collections.Generic;

namespace com.adtrace.sdk
{
    public class AdTraceEventFailure
    {
        public string Adid { get; set; }
        public string Message { get; set; }
        public string Timestamp { get; set; }
        public string EventToken { get; set; }
        public string CallbackId { get; set; }
        public bool WillRetry { get; set; }
        public Dictionary<string, object> JsonResponse { get; set; }

        public AdTraceEventFailure() {}

        public AdTraceEventFailure(Dictionary<string, string> eventFailureDataMap)
        {
            if (eventFailureDataMap == null)
            {
                return;
            }

            Adid = AdTraceUtils.TryGetValue(eventFailureDataMap, AdTraceUtils.KeyAdid);
            Message = AdTraceUtils.TryGetValue(eventFailureDataMap, AdTraceUtils.KeyMessage);
            Timestamp = AdTraceUtils.TryGetValue(eventFailureDataMap, AdTraceUtils.KeyTimestamp);
            EventToken = AdTraceUtils.TryGetValue(eventFailureDataMap, AdTraceUtils.KeyEventToken);
            CallbackId = AdTraceUtils.TryGetValue(eventFailureDataMap, AdTraceUtils.KeyCallbackId);

            bool willRetry;
            if (bool.TryParse(AdTraceUtils.TryGetValue(eventFailureDataMap, AdTraceUtils.KeyWillRetry), out willRetry))
            {
                WillRetry = willRetry;
            }

            string jsonResponseString = AdTraceUtils.TryGetValue(eventFailureDataMap, AdTraceUtils.KeyJsonResponse);
            var jsonResponseNode = JSON.Parse(jsonResponseString);
            if (jsonResponseNode != null && jsonResponseNode.AsObject != null)
            {
                JsonResponse = new Dictionary<string, object>();
                AdTraceUtils.WriteJsonResponseDictionary(jsonResponseNode.AsObject, JsonResponse);
            }
        }

        public AdTraceEventFailure(string jsonString)
        {
            var jsonNode = JSON.Parse(jsonString);
            if (jsonNode == null)
            {
                return;
            }

            Adid = AdTraceUtils.GetJsonString(jsonNode, AdTraceUtils.KeyAdid);
            Message = AdTraceUtils.GetJsonString(jsonNode, AdTraceUtils.KeyMessage);
            Timestamp = AdTraceUtils.GetJsonString(jsonNode, AdTraceUtils.KeyTimestamp);
            EventToken = AdTraceUtils.GetJsonString(jsonNode, AdTraceUtils.KeyEventToken);
            CallbackId = AdTraceUtils.GetJsonString(jsonNode, AdTraceUtils.KeyCallbackId);
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
