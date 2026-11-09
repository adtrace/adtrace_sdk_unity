using System;
using UnityEngine;

namespace io.adtrace.sdk.cafebazaar
{
    public class AdTraceCafeBazaarReferrer : MonoBehaviour
    {
        private const string errorMsgEditor = "[AdTraceCafeBazaarReferrer]: AdTrace Cafe Bazaar Referrer plugin can not be used in Editor.";
        private const string errorMsgPlatform = "[AdTraceCafeBazaarReferrer]: AdTrace Cafe Bazaar Referrer plugin can only be used in Android apps.";

        public bool startManually = true;
        public bool readCafeBazaarReferrer = false;

        void Awake()
        {
            if (IsEditor())
            {
                return;
            }

            DontDestroyOnLoad(transform.gameObject);

            if (!this.startManually)
            {
                if (this.readCafeBazaarReferrer)
                {
                    AdTraceCafeBazaarReferrer.ReadCafeBazaarReferrer();
                }
                else
                {
                    AdTraceCafeBazaarReferrer.DoNotReadCafeBazaarReferrer();
                }
            }
        }

        public static void ReadCafeBazaarReferrer()
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_ANDROID
            AdTraceCafeBazaarReferrerAndroid.ReadCafeBazaarReferrer();
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void DoNotReadCafeBazaarReferrer()
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_ANDROID
            AdTraceCafeBazaarReferrerAndroid.DoNotReadCafeBazaarReferrer();
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
