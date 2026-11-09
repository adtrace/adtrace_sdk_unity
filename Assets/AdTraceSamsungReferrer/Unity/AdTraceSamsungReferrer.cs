using System;
using UnityEngine;

namespace io.adtrace.sdk.samsung
{
    public class AdTraceSamsungReferrer : MonoBehaviour
    {
        private const string errorMsgEditor = "[AdTraceSamsungReferrer]: AdTrace Samsung Referrer plugin can not be used in Editor.";
        private const string errorMsgPlatform = "[AdTraceSamsungReferrer]: AdTrace Samsung Referrer plugin can only be used in Android apps.";

        public bool startManually = true;
        public bool readSamsungReferrer = false;

        void Awake()
        {
            if (IsEditor())
            {
                return;
            }

            DontDestroyOnLoad(transform.gameObject);

            if (!this.startManually)
            {
                if (this.readSamsungReferrer)
                {
                    AdTraceSamsungReferrer.ReadSamsungReferrer();
                }
                else
                {
                    AdTraceSamsungReferrer.DoNotReadSamsungReferrer();
                }
            }
        }

        public static void ReadSamsungReferrer()
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_ANDROID
            AdTraceSamsungReferrerAndroid.ReadSamsungReferrer();
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void DoNotReadSamsungReferrer()
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_ANDROID
            AdTraceSamsungReferrerAndroid.DoNotReadSamsungReferrer();
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
