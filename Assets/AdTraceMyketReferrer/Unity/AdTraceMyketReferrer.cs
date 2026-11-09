using System;
using UnityEngine;

namespace io.adtrace.sdk.myket
{
    public class AdTraceMyketReferrer : MonoBehaviour
    {
        private const string errorMsgEditor = "[AdTraceMyketReferrer]: AdTrace Myket Referrer plugin can not be used in Editor.";
        private const string errorMsgPlatform = "[AdTraceMyketReferrer]: AdTrace Myket Referrer plugin can only be used in Android apps.";

        public bool startManually = true;
        public bool readMyketReferrer = false;

        void Awake()
        {
            if (IsEditor())
            {
                return;
            }

            DontDestroyOnLoad(transform.gameObject);

            if (!this.startManually)
            {
                if (this.readMyketReferrer)
                {
                    AdTraceMyketReferrer.ReadMyketReferrer();
                }
                else
                {
                    AdTraceMyketReferrer.DoNotReadMyketReferrer();
                }
            }
        }

        public static void ReadMyketReferrer()
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_ANDROID
            AdTraceMyketReferrerAndroid.ReadMyketReferrer();
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void DoNotReadMyketReferrer()
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_ANDROID
            AdTraceMyketReferrerAndroid.DoNotReadMyketReferrer();
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        private static bool IsEditor()
        {
#if UNITY_EDITOR
            Debug.Log(errorMsgEditor);
            return true;
#else
            return false;
#endif
        }
    }
}
