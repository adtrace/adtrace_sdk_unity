using System;
using System.Collections.Generic;

namespace com.adtrace.sdk
{
    public class AdTraceEventSuccess
    {
        public string Adid { get; set; }
        public string Message { get; set; }
        public string Timestamp { get; set; }
        public string EventToken { get; set; }
        public string CallbackId { get; set; }

        public Dictionary<string, object> JsonResponse { get; set; }

        public AdTraceEventSuccess() {}

        public AdTraceEventSuccess(Dictionary<string, string> eventSuccessDataMap)
        {
            if (eventSuccessDataMap == null)
            {
                return;
            }

            Adid = AdTraceUtils.TryGetValue(eventSuccessDataMap, AdTraceUtils.KeyAdid);
            Message = AdTraceUtils.TryGetValue(eventSuccessDataMap, AdTraceUtils.KeyMessage);
            Timestamp = AdTraceUtils.TryGetValue(eventSuccessDataMap, AdTraceUtils.KeyTimestamp);
            EventToken = AdTraceUtils.TryGetValue(eventSuccessDataMap, AdTraceUtils.KeyEventToken);
            CallbackId = AdTraceUtils.TryGetValue(eventSuccessDataMap, AdTraceUtils.KeyCallbackId);

            string jsonResponseString = AdTraceUtils.TryGetValue(eventSuccessDataMap, AdTraceUtils.KeyJsonResponse);
            var jsonResponseNode = JSON.Parse(jsonResponseString);
            if (jsonResponseNode != null && jsonResponseNode.AsObject != null)
            {
                JsonResponse = new Dictionary<string, object>();
                AdTraceUtils.WriteJsonResponseDictionary(jsonResponseNode.AsObject, JsonResponse);
            }
        }

        public AdTraceEventSuccess(string jsonString)
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
