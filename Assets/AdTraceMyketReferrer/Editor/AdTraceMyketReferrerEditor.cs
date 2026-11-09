using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AdTraceMyketReferrerEditor
{
    private const string MenuItem0 = "Assets/AdTraceMyketReferrer/Autorun post-build tasks";
    private const string MenuItem1 = "Assets/AdTraceMyketReferrer/Export Unity package";

    private static bool shouldAutorun = true;

    static AdTraceMyketReferrerEditor()
    {
        EditorApplication.delayCall += () =>
        {
            shouldAutorun = EditorPrefs.GetBool(MenuItem0, true);
            Menu.SetChecked(MenuItem0, shouldAutorun);
        };
    }

    [MenuItem(MenuItem0)]
    static void AutorunPostBuildTasks()
    {
        shouldAutorun = !shouldAutorun;
        Menu.SetChecked(MenuItem0, shouldAutorun);
        EditorPrefs.SetBool(MenuItem0, shouldAutorun);

        if (shouldAutorun)
        {
            EditorUtility.DisplayDialog("AdTrace Myket Referrer Plugin", "AdTrace Myket Referrer plugin post build tasks will from now on be performed each time you build your app.", "OK");
        }
        else
        {
            EditorUtility.DisplayDialog("AdTrace Myket Referrer Plugin", "AdTrace Myket Referrer plugin post build tasks from now on will not be performed each time you build your app.", "OK");
        }
    }

    [MenuItem(MenuItem1)]
    static void ExportAdTraceMyketReferrerPackage()
    {
        string exportedFileName = "AdTraceMyketReferrer.unitypackage";
        string assetsPath = "Assets/AdTraceMyketReferrer";
        List<string> assetsToExport = new List<string>();

        assetsToExport.Add(assetsPath + "/Android/adtrace-android-myket-referrer.aar");
        assetsToExport.Add(assetsPath + "/Android/myket-referrersdk-1.0.3.aar");
        assetsToExport.Add(assetsPath + "/Android/AdTraceMyketReferrerAndroid.cs");
        assetsToExport.Add("Assets/AdTraceStoreReferrer/Android/kotlin-stdlib.jar");
        assetsToExport.Add(assetsPath + "/Editor/AdTraceMyketReferrerEditor.cs");
        assetsToExport.Add(assetsPath + "/Prefab/AdTraceMyketReferrer.prefab");
        assetsToExport.Add(assetsPath + "/Unity/AdTraceMyketReferrer.cs");

        AssetDatabase.ExportPackage(
            assetsToExport.ToArray(),
            exportedFileName,
            ExportPackageOptions.IncludeDependencies | ExportPackageOptions.Interactive);
    }
}
