using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AdTraceCafeBazaarReferrerEditor
{
    private const string MenuItem0 = "Assets/AdTraceCafeBazaarReferrer/Autorun post-build tasks";
    private const string MenuItem1 = "Assets/AdTraceCafeBazaarReferrer/Export Unity package";

    private static bool shouldAutorun = true;

    static AdTraceCafeBazaarReferrerEditor()
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
            EditorUtility.DisplayDialog("AdTrace Cafe Bazaar Referrer Plugin", "AdTrace Cafe Bazaar Referrer plugin post build tasks will from now on be performed each time you build your app.", "OK");
        }
        else
        {
            EditorUtility.DisplayDialog("AdTrace Cafe Bazaar Referrer Plugin", "AdTrace Cafe Bazaar Referrer plugin post build tasks from now on will not be performed each time you build your app.", "OK");
        }
    }

    [MenuItem(MenuItem1)]
    static void ExportAdTraceCafeBazaarReferrerPackage()
    {
        string exportedFileName = "AdTraceCafeBazaarReferrer.unitypackage";
        string assetsPath = "Assets/AdTraceCafeBazaarReferrer";
        List<string> assetsToExport = new List<string>();

        assetsToExport.Add(assetsPath + "/Android/adtrace-android-cafebazaar-referrer.aar");
        assetsToExport.Add(assetsPath + "/Android/cafebazaar-referrersdk-1.1.1.aar");
        assetsToExport.Add(assetsPath + "/Android/cafebazaar-servicebase-1.0.0.aar");
        assetsToExport.Add(assetsPath + "/Android/AdTraceCafeBazaarReferrerAndroid.cs");
        assetsToExport.Add("Assets/AdTraceStoreReferrer/Android/kotlin-stdlib.jar");
        assetsToExport.Add(assetsPath + "/Editor/AdTraceCafeBazaarReferrerEditor.cs");
        assetsToExport.Add(assetsPath + "/Prefab/AdTraceCafeBazaarReferrer.prefab");
        assetsToExport.Add(assetsPath + "/Unity/AdTraceCafeBazaarReferrer.cs");

        AssetDatabase.ExportPackage(
            assetsToExport.ToArray(),
            exportedFileName,
            ExportPackageOptions.IncludeDependencies | ExportPackageOptions.Interactive);
    }
}
