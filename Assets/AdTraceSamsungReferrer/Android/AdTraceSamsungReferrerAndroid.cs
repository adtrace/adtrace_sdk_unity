using UnityEngine;

namespace io.adtrace.sdk.samsung
{
#if UNITY_ANDROID
    public class AdTraceSamsungReferrerAndroid
    {
        private static AndroidJavaClass ajcSamsungReferrer = new AndroidJavaClass("io.adtrace.sdk.samsung.AdTraceSamsungReferrer");
        private static AndroidJavaObject ajoCurrentActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");

        public static void ReadSamsungReferrer()
        {
            if (ajcSamsungReferrer == null)
            {
                ajcSamsungReferrer = new AndroidJavaClass("io.adtrace.sdk.samsung.AdTraceSamsungReferrer");
            }
            ajcSamsungReferrer.CallStatic("readSamsungReferrer", ajoCurrentActivity);
        }

        public static void DoNotReadSamsungReferrer()
        {
            if (ajcSamsungReferrer == null)
            {
                ajcSamsungReferrer = new AndroidJavaClass("io.adtrace.sdk.samsung.AdTraceSamsungReferrer");
            }
            ajcSamsungReferrer.CallStatic("doNotReadSamsungReferrer");
        }
    }
#endif
}
