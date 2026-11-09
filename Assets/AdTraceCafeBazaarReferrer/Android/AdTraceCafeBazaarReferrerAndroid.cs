using System;
using UnityEngine;

namespace io.adtrace.sdk.cafebazaar
{
#if UNITY_ANDROID
    public class AdTraceCafeBazaarReferrerAndroid
    {
        private static AndroidJavaClass ajcCafeBazaarReferrer = new AndroidJavaClass("io.adtrace.sdk.cafebazaar.AdTraceCafeBazaarReferrer");
        private static AndroidJavaObject ajoCurrentActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");

        public static void ReadCafeBazaarReferrer()
        {
            if (ajcCafeBazaarReferrer == null)
            {
                ajcCafeBazaarReferrer = new AndroidJavaClass("io.adtrace.sdk.cafebazaar.AdTraceCafeBazaarReferrer");
            }
            ajcCafeBazaarReferrer.CallStatic("readCafeBazaarReferrer", ajoCurrentActivity);
        }

        public static void DoNotReadCafeBazaarReferrer()
        {
            if (ajcCafeBazaarReferrer == null)
            {
                ajcCafeBazaarReferrer = new AndroidJavaClass("io.adtrace.sdk.cafebazaar.AdTraceCafeBazaarReferrer");
            }
            ajcCafeBazaarReferrer.CallStatic("doNotReadCafeBazaarReferrer");
        }
    }
#endif
}
