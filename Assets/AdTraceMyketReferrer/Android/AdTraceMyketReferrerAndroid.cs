using System;
using UnityEngine;

namespace io.adtrace.sdk.myket
{
#if UNITY_ANDROID
    public class AdTraceMyketReferrerAndroid
    {
        private static AndroidJavaClass ajcMyketReferrer = new AndroidJavaClass("io.adtrace.sdk.myket.AdTraceMyketReferrer");
        private static AndroidJavaObject ajoCurrentActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");

        public static void ReadMyketReferrer()
        {
            if (ajcMyketReferrer == null)
            {
                ajcMyketReferrer = new AndroidJavaClass("io.adtrace.sdk.myket.AdTraceMyketReferrer");
            }
            ajcMyketReferrer.CallStatic("readMyketReferrer", ajoCurrentActivity);
        }

        public static void DoNotReadMyketReferrer()
        {
            if (ajcMyketReferrer == null)
            {
                ajcMyketReferrer = new AndroidJavaClass("io.adtrace.sdk.myket.AdTraceMyketReferrer");
            }
            ajcMyketReferrer.CallStatic("doNotReadMyketReferrer");
        }
    }
#endif
}
