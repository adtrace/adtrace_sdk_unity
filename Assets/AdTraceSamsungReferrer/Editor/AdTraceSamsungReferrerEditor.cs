using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AdTraceSamsungReferrerEditor
{
    private const string MenuItem0 = "Assets/AdTraceSamsungReferrer/Autorun post-build tasks";
    private const string MenuItem1 = "Assets/AdTraceSamsungReferrer/Export Unity package";

    private static bool shouldAutorun = true;

    static AdTraceSamsungReferrerEditor()
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
            EditorUtility.DisplayDialog("AdTrace Samsung Referrer Plugin", "AdTrace Samsung Referrer plugin post build tasks will from now on be performed each time you build your app.", "OK");
        }
        else
        {
            EditorUtility.DisplayDialog("AdTrace Samsung Referrer Plugin", "AdTrace Samsung Referrer plugin post build tasks from now on will not be performed each time you build your app.", "OK");
        }
    }

    [MenuItem(MenuItem1)]
    static void ExportAdTraceSamsungReferrerPackage()
    {
        string exportedFileName = "AdTraceSamsungReferrer.unitypackage";
        string assetsPath = "Assets/AdTraceSamsungReferrer";
        List<string> assetsToExport = new List<string>();

        assetsToExport.Add(assetsPath + "/Android/adtrace-android-samsung-referrer.aar");
        assetsToExport.Add(assetsPath + "/Android/samsung-galaxystore-install-referrer-3.0.1.aar");
        assetsToExport.Add(assetsPath + "/Android/AdTraceSamsungReferrerAndroid.cs");
        assetsToExport.Add(assetsPath + "/Editor/AdTraceSamsungReferrerEditor.cs");
        assetsToExport.Add(assetsPath + "/Prefab/AdTraceSamsungReferrer.prefab");
        assetsToExport.Add(assetsPath + "/Unity/AdTraceSamsungReferrer.cs");

        AssetDatabase.ExportPackage(
            assetsToExport.ToArray(),
            exportedFileName,
            ExportPackageOptions.IncludeDependencies | ExportPackageOptions.Interactive);
    }
}
