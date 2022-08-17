using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

public class AdTraceEditor : AssetPostprocessor
{
    [MenuItem("Assets/AdTrace/Export Unity Package")]
    public static void ExportAdTraceUnityPackage()
    {
        string exportedFileName = "AdTrace.unitypackage";
        string assetsPath = "Assets/AdTrace";
        List<string> assetsToExport = new List<string>();

        // AdTrace Assets.
        assetsToExport.Add(assetsPath + "/3rd Party/SimpleJSON.cs");

        assetsToExport.Add(assetsPath + "/Android/adtrace-android.jar");
        assetsToExport.Add(assetsPath + "/Android/AdTraceAndroid.cs");
        assetsToExport.Add(assetsPath + "/Android/AdTraceAndroidManifest.xml");

        assetsToExport.Add(assetsPath + "/Editor/AdTraceEditor.cs");
        assetsToExport.Add(assetsPath + "/Editor/AdTraceSettings.cs");
        assetsToExport.Add(assetsPath + "/Editor/AdTraceSettingsEditor.cs");
        assetsToExport.Add(assetsPath + "/Editor/AdTraceCustomEditor.cs");
        assetsToExport.Add(assetsPath + "/Editor/AdTraceEditorPreprocessor.cs");

        assetsToExport.Add(assetsPath + "/ExampleGUI/ExampleGUI.cs");
        assetsToExport.Add(assetsPath + "/ExampleGUI/ExampleGUI.prefab");
        assetsToExport.Add(assetsPath + "/ExampleGUI/ExampleGUI.unity");

        assetsToExport.Add(assetsPath + "/iOS/ADTAttribution.h");
        assetsToExport.Add(assetsPath + "/iOS/ADTConfig.h");
        assetsToExport.Add(assetsPath + "/iOS/ADTEvent.h");
        assetsToExport.Add(assetsPath + "/iOS/ADTEventFailure.h");
        assetsToExport.Add(assetsPath + "/iOS/ADTEventSuccess.h");
        assetsToExport.Add(assetsPath + "/iOS/ADTLogger.h");
        assetsToExport.Add(assetsPath + "/iOS/ADTSessionFailure.h");
        assetsToExport.Add(assetsPath + "/iOS/ADTSessionSuccess.h");
        assetsToExport.Add(assetsPath + "/iOS/ADTSubscription.h");
        assetsToExport.Add(assetsPath + "/iOS/AdTrace.h");
        assetsToExport.Add(assetsPath + "/iOS/AdTraceiOS.cs");
        assetsToExport.Add(assetsPath + "/iOS/AdTraceSdk.a");
        assetsToExport.Add(assetsPath + "/iOS/AdTraceUnity.h");
        assetsToExport.Add(assetsPath + "/iOS/AdTraceUnity.mm");
        assetsToExport.Add(assetsPath + "/iOS/AdTraceUnityDelegate.h");
        assetsToExport.Add(assetsPath + "/iOS/AdTraceUnityDelegate.mm");

        assetsToExport.Add(assetsPath + "/Prefab/AdTrace.prefab");

        assetsToExport.Add(assetsPath + "/Unity/AdTrace.cs");
        assetsToExport.Add(assetsPath + "/Unity/AdTraceAppStoreSubscription.cs");
        assetsToExport.Add(assetsPath + "/Unity/AdTraceAttribution.cs");
        assetsToExport.Add(assetsPath + "/Unity/AdTraceConfig.cs");
        assetsToExport.Add(assetsPath + "/Unity/AdTraceEnvironment.cs");
        assetsToExport.Add(assetsPath + "/Unity/AdTraceEvent.cs");
        assetsToExport.Add(assetsPath + "/Unity/AdTraceEventFailure.cs");
        assetsToExport.Add(assetsPath + "/Unity/AdTraceEventSuccess.cs");
        assetsToExport.Add(assetsPath + "/Unity/AdTraceLogLevel.cs");
        assetsToExport.Add(assetsPath + "/Unity/AdTracePlayStoreSubscription.cs");
        assetsToExport.Add(assetsPath + "/Unity/AdTraceSessionFailure.cs");
        assetsToExport.Add(assetsPath + "/Unity/AdTraceSessionSuccess.cs");
        assetsToExport.Add(assetsPath + "/Unity/AdTraceUtils.cs");

        assetsToExport.Add(assetsPath + "/Windows/AdTraceWindows.cs");
        assetsToExport.Add(assetsPath + "/Windows/WindowsPcl.dll");
        assetsToExport.Add(assetsPath + "/Windows/WindowsUap.dll");
        assetsToExport.Add(assetsPath + "/Windows/Stubs/Win10Interface.dll");
        assetsToExport.Add(assetsPath + "/Windows/Stubs/Win81Interface.dll");
        assetsToExport.Add(assetsPath + "/Windows/Stubs/WinWsInterface.dll");
        assetsToExport.Add(assetsPath + "/Windows/W81/AdTraceWP81.dll");
        assetsToExport.Add(assetsPath + "/Windows/W81/Win81Interface.dll");
        assetsToExport.Add(assetsPath + "/Windows/WS/AdTraceWS.dll");
        assetsToExport.Add(assetsPath + "/Windows/WS/WinWsInterface.dll");
        assetsToExport.Add(assetsPath + "/Windows/WU10/AdTraceUAP10.dll");
        assetsToExport.Add(assetsPath + "/Windows/WU10/Win10Interface.dll");
        assetsToExport.Add(assetsPath + "/Windows/Newtonsoft.Json.dll");

        AssetDatabase.ExportPackage(
            assetsToExport.ToArray(),
            exportedFileName,
            ExportPackageOptions.IncludeDependencies | ExportPackageOptions.Interactive);
    }
    
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string projectPath)
    {
        RunPostBuildScript(target: target, projectPath: projectPath);
    }

    private static void RunPostBuildScript(BuildTarget target, string projectPath = "")
    {
        if (target == BuildTarget.iOS)
        {
#if UNITY_IOS
            Debug.Log("[AdTrace]: Starting to perform post build tasks for iOS platform.");

            string xcodeProjectPath = projectPath + "/Unity-iPhone.xcodeproj/project.pbxproj";

            PBXProject xcodeProject = new PBXProject();
            xcodeProject.ReadFromFile(xcodeProjectPath);

#if UNITY_2019_3_OR_NEWER
            string xcodeTarget = xcodeProject.GetUnityMainTargetGuid();
#else
            string xcodeTarget = xcodeProject.TargetGuidByName("Unity-iPhone");
#endif
            HandlePlistIosChanges(projectPath);

            if (AdTraceSettings.iOSUniversalLinksDomains.Length > 0)
            {
                AddUniversalLinkDomains(xcodeProject, xcodeProjectPath, xcodeTarget);
            }

            // If enabled by the user, AdTrace SDK will try to add following frameworks to your project:
            // - AdSupport.framework (needed for access to IDFA value)
            // - iAd.framework (needed in case you are running ASA campaigns)
            // - AdServices.framework (needed in case you are running ASA campaigns)
            // - StoreKit.framework (needed for communication with SKAdNetwork framework)
            // - AppTrackingTransparency.framework (needed for information about user's consent to be tracked)
            // In case you don't need any of these, feel free to remove them from your app.

            if (AdTraceSettings.iOSFrameworkAdSupport)
            {
                Debug.Log("[AdTrace]: Adding AdSupport.framework to Xcode project.");
                xcodeProject.AddFrameworkToProject(xcodeTarget, "AdSupport.framework", true);
                Debug.Log("[AdTrace]: AdSupport.framework added successfully.");
            }
            else
            {
                Debug.Log("[AdTrace]: Skipping AdSupport.framework linking.");
            }
            if (AdTraceSettings.iOSFrameworkiAd)
            {
                Debug.Log("[AdTrace]: Adding iAd.framework to Xcode project.");
                xcodeProject.AddFrameworkToProject(xcodeTarget, "iAd.framework", true);
                Debug.Log("[AdTrace]: iAd.framework added successfully.");
            }
            else
            {
                Debug.Log("[AdTrace]: Skipping iAd.framework linking.");
            }
            if (AdTraceSettings.iOSFrameworkAdServices)
            {
                Debug.Log("[AdTrace]: Adding AdServices.framework to Xcode project.");
                xcodeProject.AddFrameworkToProject(xcodeTarget, "AdServices.framework", true);
                Debug.Log("[AdTrace]: AdServices.framework added successfully.");
            }
            else
            {
                Debug.Log("[AdTrace]: Skipping AdServices.framework linking.");
            }
            if (AdTraceSettings.iOSFrameworkStoreKit)
            {
                Debug.Log("[AdTrace]: Adding StoreKit.framework to Xcode project.");
                xcodeProject.AddFrameworkToProject(xcodeTarget, "StoreKit.framework", true);
                Debug.Log("[AdTrace]: StoreKit.framework added successfully.");
            }
            else
            {
                Debug.Log("[AdTrace]: Skipping StoreKit.framework linking.");
            }
            if (AdTraceSettings.iOSFrameworkAppTrackingTransparency)
            {
                Debug.Log("[AdTrace]: Adding AppTrackingTransparency.framework to Xcode project.");
                xcodeProject.AddFrameworkToProject(xcodeTarget, "AppTrackingTransparency.framework", true);
                Debug.Log("[AdTrace]: AppTrackingTransparency.framework added successfully.");
            }
            else
            {
                Debug.Log("[AdTrace]: Skipping AppTrackingTransparency.framework linking.");
            }

            // The AdTrace SDK needs to have Obj-C exceptions enabled.
            // GCC_ENABLE_OBJC_EXCEPTIONS=YES
            Debug.Log("[AdTrace]: Enabling Obj-C exceptions by setting GCC_ENABLE_OBJC_EXCEPTIONS value to YES.");
            xcodeProject.AddBuildProperty(xcodeTarget, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");
            Debug.Log("[AdTrace]: Obj-C exceptions enabled successfully.");

            // The AdTrace SDK needs to have -ObjC flag set in other linker flags section because of it's categories.
            // OTHER_LDFLAGS -ObjC
            //
            // Seems that in newer Unity IDE versions adding -ObjC flag to Unity-iPhone target doesn't do the trick.
            // Adding -ObjC to UnityFramework target however does make things work nicely again.
            // This happens because Unity is linking SDK's static library into UnityFramework target.
            // Check for presence of UnityFramework target and if there, include -ObjC flag inside of it.

            Debug.Log("[AdTrace]: Adding -ObjC flag to other linker flags (OTHER_LDFLAGS) of Unity-iPhone target.");
            xcodeProject.AddBuildProperty(xcodeTarget, "OTHER_LDFLAGS", "-ObjC");
            Debug.Log("[AdTrace]: -ObjC successfully added to other linker flags.");
            string xcodeTargetUnityFramework = xcodeProject.TargetGuidByName("UnityFramework");
            if (!string.IsNullOrEmpty(xcodeTargetUnityFramework))
            {
                Debug.Log("[AdTrace]: Adding -ObjC flag to other linker flags (OTHER_LDFLAGS) of UnityFramework target.");
                xcodeProject.AddBuildProperty(xcodeTargetUnityFramework, "OTHER_LDFLAGS", "-ObjC");
                Debug.Log("[AdTrace]: -ObjC successfully added to other linker flags.");
            }

            if (xcodeProject.ContainsFileByProjectPath("Libraries/AdTrace/iOS/AdTraceSigSdk.a"))
            {
                xcodeProject.AddBuildProperty(xcodeTarget, "OTHER_LDFLAGS", "-force_load $(PROJECT_DIR)/Libraries/AdTrace/iOS/AdTraceSigSdk.a");
            }

            // Save the changes to Xcode project file.
            xcodeProject.WriteToFile(xcodeProjectPath);
#endif
        }
    }

#if UNITY_IOS
    private static void HandlePlistIosChanges(string projectPath)
    {
        const string UserTrackingUsageDescriptionKey = "NSUserTrackingUsageDescription";

        // Check if needs to do any info plist change.
        bool hasUserTrackingDescription =
            !string.IsNullOrEmpty(AdTraceSettings.iOSUserTrackingUsageDescription);
        bool hasUrlSchemesDeepLinksEnabled = AdTraceSettings.iOSUrlSchemes.Length > 0;

        if (!hasUserTrackingDescription && !hasUrlSchemesDeepLinksEnabled)
        {
            return;
        }

        // Get and read info plist.
        var plistPath = Path.Combine(projectPath, "Info.plist");
        var plist = new PlistDocument();
        plist.ReadFromFile(plistPath);
        var plistRoot = plist.root;

        // Do the info plist changes.
        if (hasUserTrackingDescription)
        {
            if (plistRoot[UserTrackingUsageDescriptionKey] != null)
            {
                Debug.Log("[AdTrace]: Overwritting User Tracking Usage Description.");
            }
            plistRoot.SetString(UserTrackingUsageDescriptionKey,
                AdTraceSettings.iOSUserTrackingUsageDescription);
        }

        if (hasUrlSchemesDeepLinksEnabled)
        {
            AddUrlSchemesIOS(plistRoot, AdTraceSettings.iOSUrlIdentifier, AdTraceSettings.iOSUrlSchemes);
        }

        // Write any info plist change.
        File.WriteAllText(plistPath, plist.WriteToString());
    }

    private static void AddUrlSchemesIOS(PlistElementDict plistRoot, string urlIdentifier, string[] urlSchemes)
    {
        // Set Array for futher deeplink values.
        var urlTypesArray = CreatePlistElementArray(plistRoot, "CFBundleURLTypes");

        // Array will contain just one deeplink dictionary
        var urlSchemesItems = CreatePlistElementDict(urlTypesArray);
        urlSchemesItems.SetString("CFBundleURLName", urlIdentifier);
        var urlSchemesArray = CreatePlistElementArray(urlSchemesItems, "CFBundleURLSchemes");

        // Delete old deferred deeplinks URIs
        Debug.Log("[AdTrace]: Removing deeplinks that already exist in the array to avoid duplicates.");
        foreach (var link in urlSchemes)
        {
            urlSchemesArray.values.RemoveAll(
                element => element != null && element.AsString().Equals(link));
        }

        Debug.Log("[AdTrace]: Adding new deep links.");
        foreach (var link in urlSchemes.Distinct())
        {
            urlSchemesArray.AddString(link);
        }
    }

    private static PlistElementArray CreatePlistElementArray(PlistElementDict root, string key)
    {
        if (!root.values.ContainsKey(key))
        {
            Debug.Log(string.Format("[AdTrace]: {0} not found in Info.plist. Creating a new one.", key));
            return root.CreateArray(key);
        }
        var result = root.values[key].AsArray();
        return result != null ? result : root.CreateArray(key);
    }

    private static PlistElementDict CreatePlistElementDict(PlistElementArray rootArray)
    {
        if (rootArray.values.Count == 0)
        {
            Debug.Log("[AdTrace]: Deeplinks array doesn't contain dictionary for deeplinks. Creating a new one.");
            return rootArray.AddDict();
        }

        var urlSchemesItems = rootArray.values[0].AsDict();
        Debug.Log("[AdTrace]: Reading deeplinks array");
        if (urlSchemesItems == null)
        {
            Debug.Log("[AdTrace]: Deeplinks array doesn't contain dictionary for deeplinks. Creating a new one.");
            urlSchemesItems = rootArray.AddDict();
        }

        return urlSchemesItems;
    }

    private static void AddUniversalLinkDomains(PBXProject project, string xCodeProjectPath, string xCodeTarget)
    {
        string entitlementsFileName = "Unity-iPhone.entitlements";

        Debug.Log("[AdTrace]: Adding associated domains to entitlements file.");
#if UNITY_2019_3_OR_NEWER
        var projectCapabilityManager = new ProjectCapabilityManager(xCodeProjectPath, entitlementsFileName, null, project.GetUnityMainTargetGuid());
#else
        var projectCapabilityManager = new ProjectCapabilityManager(xCodeProjectPath, entitlementsFileName, PBXProject.GetUnityTargetName());
#endif
        var uniqueDomains = AdTraceSettings.iOSUniversalLinksDomains.Distinct().ToArray();
        const string applinksPrefix = "applinks:";
        for (int i = 0; i < uniqueDomains.Length; i++)
        {
            if (!uniqueDomains[i].StartsWith(applinksPrefix))
            {
                uniqueDomains[i] = applinksPrefix + uniqueDomains[i];
            }
        }

        projectCapabilityManager.AddAssociatedDomains(uniqueDomains);
        projectCapabilityManager.WriteToFile();

        Debug.Log("[AdTrace]: Enabling Associated Domains capability with created entitlements file.");
        project.AddCapability(xCodeTarget, PBXCapabilityType.AssociatedDomains, entitlementsFileName);
    }
#endif
}
