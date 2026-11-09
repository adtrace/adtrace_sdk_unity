using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS || UNITY_TVOS
using UnityEditor.iOS.Xcode;
#endif

public class AdTraceEditor : AssetPostprocessor
{
    [MenuItem("Assets/AdTrace/Check iOS 14 Support Status")]
    public static void CheckIOS14SupportStatus()
    {
        EditorUtility.DisplayDialog("AdTrace SDK", "iOS 14 support is " + (AdTraceSettings.IsiOS14ProcessingEnabled ? "enabled." : "disabled."), "OK");
    }

    [MenuItem("Assets/AdTrace/Toggle iOS 14 Support Status")]
    public static void ToggleiOS14SupportStatus()
    {
        AdTraceSettings.IsiOS14ProcessingEnabled = !AdTraceSettings.IsiOS14ProcessingEnabled;
        EditorUtility.SetDirty(AdTraceSettings.Instance);
        EditorUtility.DisplayDialog("AdTrace SDK", "iOS 14 support is now " + (AdTraceSettings.IsiOS14ProcessingEnabled ? "enabled." : "disabled.") +
            "\nNote: Make sure to save your project files in order for this change to take effect.", "OK");
    }

    [MenuItem("Assets/AdTrace/Check Post Processing Status")]
    public static void CheckPostProcessingStatus()
    {
        EditorUtility.DisplayDialog("AdTrace SDK", "The post processing for AdTrace SDK is " + (AdTraceSettings.IsPostProcessingEnabled ? "enabled." : "disabled."), "OK");
    }

    [MenuItem("Assets/AdTrace/Toggle Post Processing Status")]
    public static void TogglePostProcessingStatus()
    {
        AdTraceSettings.IsPostProcessingEnabled = !AdTraceSettings.IsPostProcessingEnabled;
        EditorUtility.SetDirty(AdTraceSettings.Instance);
        EditorUtility.DisplayDialog("AdTrace SDK", "The post processing for AdTrace SDK is now " + (AdTraceSettings.IsPostProcessingEnabled ? "enabled." : "disabled.") +
            "\nNote: Make sure to save your project files in order for this change to take effect.", "OK");
    }

    [MenuItem("Assets/AdTrace/Export Unity Package")]
    public static void ExportAdTraceUnityPackage()
    {
        string exportedFileName = "AdTrace.unitypackage";
        string assetsPath = "Assets/AdTrace";
        List<string> assetsToExport = new List<string>();

        // AdTrace Assets.
        assetsToExport.Add(assetsPath + "/3rd Party/SimpleJSON.cs");

        assetsToExport.Add(assetsPath + "/Android/adtrace-android.aar");
        assetsToExport.Add(assetsPath + "/Android/AdTraceAndroid.cs");
        assetsToExport.Add(assetsPath + "/Android/AdTraceAndroidManifest.xml");

        assetsToExport.Add(assetsPath + "/Editor/AdTraceEditor.cs");
        assetsToExport.Add(assetsPath + "/Editor/AdTraceSettings.cs");

        assetsToExport.Add(assetsPath + "/ExampleGUI/ExampleGUI.cs");
        assetsToExport.Add(assetsPath + "/ExampleGUI/ExampleGUI.prefab");
        assetsToExport.Add(assetsPath + "/ExampleGUI/ExampleGUI.unity");

        assetsToExport.Add(assetsPath + "/iOS/ADTAttribution.h");
        assetsToExport.Add(assetsPath + "/iOS/ADTConfig.h");
        assetsToExport.Add(assetsPath + "/iOS/ADTEvent.h");
        assetsToExport.Add(assetsPath + "/iOS/ADTEventFailure.h");
        assetsToExport.Add(assetsPath + "/iOS/ADTEventSuccess.h");
        assetsToExport.Add(assetsPath + "/iOS/ADTLogger.h");
        assetsToExport.Add(assetsPath + "/iOS/ADTPurchase.h");
        assetsToExport.Add(assetsPath + "/iOS/ADTPurchaseVerificationResult.h");
        assetsToExport.Add(assetsPath + "/iOS/PrivacyInfo.xcprivacy");
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
        // Check what is user setting about allowing AdTrace SDK to perform post build tasks.
        // If user disabled it, oh well, we won't do a thing.
        if (!AdTraceSettings.IsPostProcessingEnabled)
        {
            UnityEngine.Debug.Log("[AdTrace]: You have forbidden the AdTrace SDK to perform post processing tasks.");
            UnityEngine.Debug.Log("[AdTrace]: Skipping post processing tasks.");
            return;
        }

        RunPostBuildScript(target:target, preBuild:false, projectPath:projectPath);
    }

    private static void RunPostBuildScript(BuildTarget target, bool preBuild, string projectPath = "")
    {
        if (target == BuildTarget.Android)
        {
            UnityEngine.Debug.Log("[AdTrace]: Starting to perform post build tasks for Android platform.");
            RunPostProcessTasksAndroid();
        }
        else if (target == BuildTarget.iOS || target == BuildTarget.tvOS)
        {
#if UNITY_IOS || UNITY_TVOS
            bool isTvOs = target == BuildTarget.tvOS;
            UnityEngine.Debug.Log("[AdTrace]: Starting to perform post build tasks for " + (isTvOs ? "tvOS" : "iOS") + " platform.");
            
            string xcodeProjectPath = projectPath + "/Unity-iPhone.xcodeproj/project.pbxproj";

            PBXProject xcodeProject = new PBXProject();
            xcodeProject.ReadFromFile(xcodeProjectPath);

            // The AdTrace SDK will try to add following frameworks to your project:
            // - AdSupport.framework (needed for access to IDFA value)
            // - iAd.framework (needed in case you are running ASA campaigns)
            // - AdServices.framework (needed in case you are running ASA campaigns)
            // - CoreTelephony.framework (needed to get information about network type user is connected to)
            // - StoreKit.framework (needed for communication with SKAdNetwork framework)
            // - AppTrackingTransparency.framework (needed for information about user's consent to be tracked)

            // In case you don't need any of these, feel free to remove them from your app.

#if UNITY_2019_3_OR_NEWER
            string xcodeTarget = xcodeProject.GetUnityFrameworkTargetGuid();
#else
            string xcodeTarget = xcodeProject.TargetGuidByName("Unity-iPhone");
#endif
            
            UnityEngine.Debug.Log("[AdTrace]: Adding AdSupport.framework to Xcode project.");
            xcodeProject.AddFrameworkToProject(xcodeTarget, "AdSupport.framework", true);
            UnityEngine.Debug.Log("[AdTrace]: AdSupport.framework added successfully.");

            UnityEngine.Debug.Log("[AdTrace]: Adding iAd.framework to Xcode project.");
            xcodeProject.AddFrameworkToProject(xcodeTarget, "iAd.framework", true);
            UnityEngine.Debug.Log("[AdTrace]: iAd.framework added successfully.");

            UnityEngine.Debug.Log("[AdTrace]: Adding AdServices.framework to Xcode project.");
            xcodeProject.AddFrameworkToProject(xcodeTarget, "AdServices.framework", true);
            UnityEngine.Debug.Log("[AdTrace]: AdServices.framework added successfully.");

            UnityEngine.Debug.Log("[AdTrace]: Adding CoreTelephony.framework to Xcode project.");
            xcodeProject.AddFrameworkToProject(xcodeTarget, "CoreTelephony.framework", true);
            UnityEngine.Debug.Log("[AdTrace]: CoreTelephony.framework added successfully.");

            if (AdTraceSettings.IsiOS14ProcessingEnabled)
            {
                UnityEngine.Debug.Log("[AdTrace]: Xcode project being built with iOS 14 support.");

                UnityEngine.Debug.Log("[AdTrace]: Adding StoreKit.framework to Xcode project.");
                xcodeProject.AddFrameworkToProject(xcodeTarget, "StoreKit.framework", true);
                UnityEngine.Debug.Log("[AdTrace]: StoreKit.framework added successfully.");

                UnityEngine.Debug.Log("[AdTrace]: Adding AppTrackingTransparency.framework to Xcode project.");
                xcodeProject.AddFrameworkToProject(xcodeTarget, "AppTrackingTransparency.framework", true);
                UnityEngine.Debug.Log("[AdTrace]: AppTrackingTransparency.framework added successfully.");
            }

            // The AdTrace SDK needs to have Obj-C exceptions enabled.
            // GCC_ENABLE_OBJC_EXCEPTIONS=YES

            UnityEngine.Debug.Log("[AdTrace]: Enabling Obj-C exceptions by setting GCC_ENABLE_OBJC_EXCEPTIONS value to YES.");
            xcodeProject.AddBuildProperty(xcodeTarget, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");

            UnityEngine.Debug.Log("[AdTrace]: Obj-C exceptions enabled successfully.");

            // The AdTrace SDK needs to have -ObjC flag set in other linker flags section because of it's categories.
            // OTHER_LDFLAGS -ObjC
            
            UnityEngine.Debug.Log("[AdTrace]: Adding -ObjC flag to other linker flags (OTHER_LDFLAGS).");
            xcodeProject.AddBuildProperty(xcodeTarget, "OTHER_LDFLAGS", "-ObjC");

            UnityEngine.Debug.Log("[AdTrace]: -ObjC successfully added to other linker flags.");

            if (xcodeProject.ContainsFileByProjectPath("Libraries/AdTrace/iOS/AdTraceSigSdk.a"))
            {
                xcodeProject.AddBuildProperty(xcodeTarget, "OTHER_LDFLAGS", "-force_load $(PROJECT_DIR)/Libraries/AdTrace/iOS/AdTraceSigSdk.a");
            }

            // iOS SDK 3.0.0 requires iOS 12.0 and tvOS 12.0.
            string deploymentTargetSetting = isTvOs ? "TVOS_DEPLOYMENT_TARGET" : "IPHONEOS_DEPLOYMENT_TARGET";
            EnsureMinimumDeploymentTarget(xcodeProject, xcodeTarget, deploymentTargetSetting, "12.0");
#if UNITY_2019_3_OR_NEWER
            string mainTarget = xcodeProject.GetUnityMainTargetGuid();
            EnsureMinimumDeploymentTarget(xcodeProject, mainTarget, deploymentTargetSetting, "12.0");
#endif

            AddPrivacyManifest(xcodeProject, projectPath, xcodeTarget);

            // Save the changes to Xcode project file.
            xcodeProject.WriteToFile(xcodeProjectPath);
#endif
        }
    }

#if UNITY_IOS || UNITY_TVOS
    private static void EnsureMinimumDeploymentTarget(PBXProject xcodeProject, string targetGuid, string settingName, string minimumVersion)
    {
        string currentValue = xcodeProject.GetBuildPropertyForAnyConfig(targetGuid, settingName);
        if (!IsVersionLower(currentValue, minimumVersion))
        {
            return;
        }

        UnityEngine.Debug.Log("[AdTrace]: Setting " + settingName + " to " + minimumVersion + ".");
        xcodeProject.SetBuildProperty(targetGuid, settingName, minimumVersion);
    }

    private static bool IsVersionLower(string currentValue, string minimumVersion)
    {
        if (string.IsNullOrEmpty(currentValue))
        {
            return true;
        }

        string[] currentParts = currentValue.Split('.');
        string[] minimumParts = minimumVersion.Split('.');
        int length = Math.Max(currentParts.Length, minimumParts.Length);
        for (int i = 0; i < length; i++)
        {
            int currentPart = 0;
            int minimumPart = 0;
            if (i < currentParts.Length)
            {
                int.TryParse(currentParts[i], out currentPart);
            }
            if (i < minimumParts.Length)
            {
                int.TryParse(minimumParts[i], out minimumPart);
            }
            if (currentPart != minimumPart)
            {
                return currentPart < minimumPart;
            }
        }

        return false;
    }

    private static void AddPrivacyManifest(PBXProject xcodeProject, string projectPath, string targetGuid)
    {
        const string fileName = "PrivacyInfo.xcprivacy";
        string sourcePath = Path.Combine(Application.dataPath, "AdTrace/iOS", fileName);
        string relativePath = "Libraries/AdTrace/iOS/" + fileName;
        string destinationPath = Path.Combine(projectPath, relativePath);

        if (!File.Exists(sourcePath))
        {
            UnityEngine.Debug.Log("[AdTrace]: PrivacyInfo.xcprivacy was not found at " + sourcePath + ".");
            return;
        }

        string destinationDirectory = Path.GetDirectoryName(destinationPath);
        if (!Directory.Exists(destinationDirectory))
        {
            Directory.CreateDirectory(destinationDirectory);
        }

        File.Copy(sourcePath, destinationPath, true);

        string fileGuid = xcodeProject.FindFileGuidByProjectPath(relativePath);
        if (string.IsNullOrEmpty(fileGuid))
        {
            fileGuid = xcodeProject.AddFile(relativePath, relativePath, PBXSourceTree.Source);
        }

        xcodeProject.AddFileToBuild(targetGuid, fileGuid);
        UnityEngine.Debug.Log("[AdTrace]: Added PrivacyInfo.xcprivacy to the Xcode project.");
    }
#endif

    private static void RunPostProcessTasksiOS(string projectPath) {}

    private static void RunPostProcessTasksAndroid()
    {
        bool isAdTraceManifestUsed = false;
        string androidPluginsPath = Path.Combine(Application.dataPath, "Plugins/Android");
        string adtraceManifestPath = Path.Combine(Application.dataPath, "AdTrace/Android/AdTraceAndroidManifest.xml");
        string appManifestPath = Path.Combine(Application.dataPath, "Plugins/Android/AndroidManifest.xml");

        // Check if user has already created AndroidManifest.xml file in its location.
        // If not, use already predefined AdTraceAndroidManifest.xml as default one.
        if (!File.Exists(appManifestPath))
        {
            if (!Directory.Exists(androidPluginsPath))
            {
                Directory.CreateDirectory(androidPluginsPath);
            }

            isAdTraceManifestUsed = true;
            File.Copy(adtraceManifestPath, appManifestPath);

            UnityEngine.Debug.Log("[AdTrace]: User defined AndroidManifest.xml file not found in Plugins/Android folder.");
            UnityEngine.Debug.Log("[AdTrace]: Creating default app's AndroidManifest.xml from AdTraceAndroidManifest.xml file.");
        }
        else
        {
            UnityEngine.Debug.Log("[AdTrace]: User defined AndroidManifest.xml file located in Plugins/Android folder.");
        }

        // If AdTrace manifest is used, we have already set up everything in it so that 
        // our native Android SDK can be used properly.
        if (!isAdTraceManifestUsed)
        {
            // However, if you already had your own AndroidManifest.xml, we'll now run
            // some checks on it and tweak it a bit if needed to add some stuff which
            // our native Android SDK needs so that it can run properly.

            // Let's open the app's AndroidManifest.xml file.
            XmlDocument manifestFile = new XmlDocument();
            manifestFile.Load(appManifestPath);
            
            bool manifestHasChanged = false;
            
            // Add needed permissions if they are missing.
            manifestHasChanged |= AddPermissions(manifestFile);

            // Add package visibility required to read Facebook and Instagram app ids.
            manifestHasChanged |= AddPackageQueries(manifestFile);

            // Add the provider that tracks sessions when the process starts.
            manifestHasChanged |= AddLifecycleProvider(manifestFile);

            // Add intent filter to main activity if it is missing.
            manifestHasChanged |= AddBroadcastReceiver(manifestFile);

            if (manifestHasChanged)
            {
                // Save the changes.
                manifestFile.Save(appManifestPath);

                // Clean the manifest file.
                CleanManifestFile(appManifestPath);

                UnityEngine.Debug.Log("[AdTrace]: App's AndroidManifest.xml file check and potential modification completed.");
                UnityEngine.Debug.Log("[AdTrace]: Please check if any error message was displayed during this process " 
                                      + "and make sure to fix all issues in order to properly use the AdTrace SDK in your app.");                
            }
            else
            {
                UnityEngine.Debug.Log("[AdTrace]: App's AndroidManifest.xml file check completed.");
                UnityEngine.Debug.Log("[AdTrace]: No modifications performed due to app's AndroidManifest.xml file compatibility.");
            }
        }
    }

    private static bool AddPermissions(XmlDocument manifest)
    {
        // The AdTrace SDK needs two permissions to be added to you app's manifest file:
        // <uses-permission android:name="android.permission.INTERNET" />
        // <uses-permission android:name="android.permission.ACCESS_WIFI_STATE" />
        // <uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
        // <uses-permission android:name="com.google.android.finsky.permission.BIND_GET_INSTALL_REFERRER_SERVICE" />
        // <uses-permission android:name="com.google.android.gms.permission.AD_ID" />

        UnityEngine.Debug.Log("[AdTrace]: Checking if all permissions needed for the AdTrace SDK are present in the app's AndroidManifest.xml file.");

        bool hasInternetPermission = false;
        bool hasAccessWifiStatePermission = false;
        bool hasAccessNetworkStatePermission = false;
        bool hasInstallReferrerServicePermission = false;
        bool hasAdIdPermission = false;

        XmlElement manifestRoot = manifest.DocumentElement;

        // Check if permissions are already there.
        foreach (XmlNode node in manifestRoot.ChildNodes)
        {
            if (node.Name == "uses-permission")
            {
                foreach (XmlAttribute attribute in node.Attributes)
                {
                    if (attribute.Value.Contains("android.permission.INTERNET"))
                    {
                        hasInternetPermission = true;
                    }
                    else if (attribute.Value.Contains("android.permission.ACCESS_WIFI_STATE"))
                    {
                        hasAccessWifiStatePermission = true;
                    }
                    else if (attribute.Value.Contains("android.permission.ACCESS_NETWORK_STATE"))
                    {
                        hasAccessNetworkStatePermission = true;
                    }
                    else if (attribute.Value.Contains("com.google.android.finsky.permission.BIND_GET_INSTALL_REFERRER_SERVICE"))
                    {
                        hasInstallReferrerServicePermission = true;
                    }
                    else if (attribute.Value.Contains("com.google.android.gms.permission.AD_ID"))
                    {
                        hasAdIdPermission = true;
                    }
                }
            }
        }

        bool manifestHasChanged = false;

        // If android.permission.INTERNET permission is missing, add it.
        if (!hasInternetPermission)
        {
            XmlElement element = manifest.CreateElement("uses-permission");
            element.SetAttribute("android__name", "android.permission.INTERNET");
            manifestRoot.AppendChild(element);
            UnityEngine.Debug.Log("[AdTrace]: android.permission.INTERNET permission successfully added to your app's AndroidManifest.xml file.");
            manifestHasChanged = true;
        }
        else
        {
            UnityEngine.Debug.Log("[AdTrace]: Your app's AndroidManifest.xml file already contains android.permission.INTERNET permission.");
        }

        // If android.permission.ACCESS_WIFI_STATE permission is missing, add it.
        if (!hasAccessWifiStatePermission)
        {
            XmlElement element = manifest.CreateElement("uses-permission");
            element.SetAttribute("android__name", "android.permission.ACCESS_WIFI_STATE");
            manifestRoot.AppendChild(element);
            UnityEngine.Debug.Log("[AdTrace]: android.permission.ACCESS_WIFI_STATE permission successfully added to your app's AndroidManifest.xml file.");
            manifestHasChanged = true;
        }
        else
        {
            UnityEngine.Debug.Log("[AdTrace]: Your app's AndroidManifest.xml file already contains android.permission.ACCESS_WIFI_STATE permission.");
        }

        // If android.permission.ACCESS_NETWORK_STATE permission is missing, add it.
        if (!hasAccessNetworkStatePermission)
        {
            XmlElement element = manifest.CreateElement("uses-permission");
            element.SetAttribute("android__name", "android.permission.ACCESS_NETWORK_STATE");
            manifestRoot.AppendChild(element);
            UnityEngine.Debug.Log("[AdTrace]: android.permission.ACCESS_NETWORK_STATE permission successfully added to your app's AndroidManifest.xml file.");
            manifestHasChanged = true;
        }
        else
        {
            UnityEngine.Debug.Log("[AdTrace]: Your app's AndroidManifest.xml file already contains android.permission.ACCESS_NETWORK_STATE permission.");
        }

        // If com.google.android.finsky.permission.BIND_GET_INSTALL_REFERRER_SERVICE permission is missing, add it.
        if (!hasInstallReferrerServicePermission)
        {
            XmlElement element = manifest.CreateElement("uses-permission");
            element.SetAttribute("android__name", "com.google.android.finsky.permission.BIND_GET_INSTALL_REFERRER_SERVICE");
            manifestRoot.AppendChild(element);
            UnityEngine.Debug.Log("[AdTrace]: com.google.android.finsky.permission.BIND_GET_INSTALL_REFERRER_SERVICE permission successfully added to your app's AndroidManifest.xml file.");
            manifestHasChanged = true;
        }
        else
        {
            UnityEngine.Debug.Log("[AdTrace]: Your app's AndroidManifest.xml file already contains com.google.android.finsky.permission.BIND_GET_INSTALL_REFERRER_SERVICE permission.");
        }

        // If com.google.android.gms.permission.AD_ID permission is missing, add it.
        if (!hasAdIdPermission)
        {
            XmlElement element = manifest.CreateElement("uses-permission");
            element.SetAttribute("android__name", "com.google.android.gms.permission.AD_ID");
            manifestRoot.AppendChild(element);
            UnityEngine.Debug.Log("[AdTrace]: com.google.android.gms.permission.AD_ID permission successfully added to your app's AndroidManifest.xml file.");
            manifestHasChanged = true;
        }
        else
        {
            UnityEngine.Debug.Log("[AdTrace]: Your app's AndroidManifest.xml file already contains com.google.android.gms.permission.AD_ID permission.");
        }

        return manifestHasChanged;
    }

    private static bool AddPackageQueries(XmlDocument manifest)
    {
        XmlElement manifestRoot = manifest.DocumentElement;
        bool hasFacebookPackage = false;
        bool hasInstagramPackage = false;

        foreach (XmlNode node in manifestRoot.ChildNodes)
        {
            if (node.Name != "queries")
            {
                continue;
            }

            foreach (XmlNode packageNode in node.ChildNodes)
            {
                if (packageNode.Name != "package" || packageNode.Attributes == null)
                {
                    continue;
                }

                foreach (XmlAttribute attribute in packageNode.Attributes)
                {
                    if (attribute.Value.Contains("com.facebook.katana"))
                    {
                        hasFacebookPackage = true;
                    }
                    else if (attribute.Value.Contains("com.instagram.android"))
                    {
                        hasInstagramPackage = true;
                    }
                }
            }
        }

        if (hasFacebookPackage && hasInstagramPackage)
        {
            return false;
        }

        XmlElement queriesElement = manifest.CreateElement("queries");
        if (!hasFacebookPackage)
        {
            XmlElement facebookPackage = manifest.CreateElement("package");
            facebookPackage.SetAttribute("android__name", "com.facebook.katana");
            queriesElement.AppendChild(facebookPackage);
        }
        if (!hasInstagramPackage)
        {
            XmlElement instagramPackage = manifest.CreateElement("package");
            instagramPackage.SetAttribute("android__name", "com.instagram.android");
            queriesElement.AppendChild(instagramPackage);
        }

        manifestRoot.AppendChild(queriesElement);
        UnityEngine.Debug.Log("[AdTrace]: Facebook and Instagram package queries added to your app's AndroidManifest.xml file.");
        return true;
    }

    private static bool AddLifecycleProvider(XmlDocument manifest)
    {
        XmlElement manifestRoot = manifest.DocumentElement;
        XmlNode applicationNode = null;

        foreach (XmlNode node in manifestRoot.ChildNodes)
        {
            if (node.Name == "application")
            {
                applicationNode = node;
                break;
            }
        }

        if (applicationNode == null)
        {
            UnityEngine.Debug.LogError("[AdTrace]: Your app's AndroidManifest.xml file does not contain \"<application>\" node.");
            UnityEngine.Debug.LogError("[AdTrace]: Unable to add the AdTrace lifecycle provider to AndroidManifest.xml.");
            return false;
        }

        foreach (XmlNode node in applicationNode.ChildNodes)
        {
            if (node.Name != "provider" || node.Attributes == null)
            {
                continue;
            }

            foreach (XmlAttribute attribute in node.Attributes)
            {
                if (attribute.Value.Contains("io.adtrace.sdk.SystemLifecycleContentProvider"))
                {
                    return false;
                }
            }
        }

        XmlElement providerElement = manifest.CreateElement("provider");
        providerElement.SetAttribute("android__name", "io.adtrace.sdk.SystemLifecycleContentProvider");
        providerElement.SetAttribute("android__authorities", "${applicationId}.adtrace-lifecycle-provider");
        providerElement.SetAttribute("android__exported", "false");
        applicationNode.AppendChild(providerElement);
        UnityEngine.Debug.Log("[AdTrace]: SystemLifecycleContentProvider successfully added to your app's AndroidManifest.xml file.");
        return true;
    }

    private static bool AddBroadcastReceiver(XmlDocument manifest)
    {
        // We're looking for existance of broadcast receiver in the AndroidManifest.xml
        // Check out the example below how that usually looks like:

        // <manifest
        //     <!-- ... -->>
        // 
        //     <supports-screens
        //         <!-- ... -->/>
        // 
        //     <application
        //         <!-- ... -->>
        //         <receiver
        //             android:name="io.adtrace.sdk.AdTraceReferrerReceiver"
        //             android:permission="android.permission.INSTALL_PACKAGES"
        //             android:exported="true" >
        //             
        //             <intent-filter>
        //                 <action android:name="com.android.vending.INSTALL_REFERRER" />
        //             </intent-filter>
        //         </receiver>
        //         
        //         <activity android:name="com.unity3d.player.UnityPlayerActivity"
        //             <!-- ... -->
        //         </activity>
        //     </application>
        // 
        //     <!-- ... -->>
        //
        // </manifest>

        UnityEngine.Debug.Log("[AdTrace]: Checking if app's AndroidManifest.xml file contains receiver for INSTALL_REFERRER intent.");

        XmlElement manifestRoot = manifest.DocumentElement;
        XmlNode applicationNode = null;

        // Let's find the application node.
        foreach(XmlNode node in manifestRoot.ChildNodes)
        {
            if (node.Name == "application")
            {
                applicationNode = node;
                break;
            }
        }

        // If there's no applicatio node, something is really wrong with your AndroidManifest.xml.
        if (applicationNode == null)
        {
            UnityEngine.Debug.LogError("[AdTrace]: Your app's AndroidManifest.xml file does not contain \"<application>\" node.");
            UnityEngine.Debug.LogError("[AdTrace]: Unable to add the AdTrace broadcast receiver to AndroidManifest.xml.");
            return false;
        }

        // Okay, there's an application node in the AndroidManifest.xml file.
        // Let's now check if user has already defined a receiver which is listening to INSTALL_REFERRER intent.
        // If that is already defined, don't force the AdTrace broadcast receiver to the manifest file.
        // If not, add the AdTrace broadcast receiver to the manifest file.

        List<XmlNode> customBroadcastReceiversNodes = getCustomRecieverNodes(applicationNode);
        if (customBroadcastReceiversNodes.Count > 0)
        {
            bool foundAdTraceBroadcastReceiver = false;
            for (int i = 0; i < customBroadcastReceiversNodes.Count; i += 1)
            {
                foreach (XmlAttribute attribute in customBroadcastReceiversNodes[i].Attributes)
                {
                    if (attribute.Value.Contains("io.adtrace.sdk.AdTraceReferrerReceiver"))
                    {
                        foundAdTraceBroadcastReceiver = true;
                    }
                }
            }

            if (!foundAdTraceBroadcastReceiver)
            {
                UnityEngine.Debug.Log("[AdTrace]: It seems like you are using your own broadcast receiver.");
                UnityEngine.Debug.Log("[AdTrace]: Please, add the calls to the AdTrace broadcast receiver like described in here: https://github.com/adtrace/android_sdk/blob/master/doc/english/referrer.md");
            }
            else
            {
                UnityEngine.Debug.Log("[AdTrace]: It seems like you are already using AdTrace broadcast receiver. Yay.");
            }

            return false;
        }
        else
        {
            // Generate AdTrace broadcast receiver entry and add it to the application node.
            XmlElement receiverElement = manifest.CreateElement("receiver");
            receiverElement.SetAttribute("android__name", "io.adtrace.sdk.AdTraceReferrerReceiver");
            receiverElement.SetAttribute("android__permission", "android.permission.INSTALL_PACKAGES");
            receiverElement.SetAttribute("android__exported", "true");

            XmlElement intentFilterElement = manifest.CreateElement("intent-filter");
            XmlElement actionElement = manifest.CreateElement("action");
            actionElement.SetAttribute("android__name", "com.android.vending.INSTALL_REFERRER");

            intentFilterElement.AppendChild(actionElement);
            receiverElement.AppendChild(intentFilterElement);
            applicationNode.AppendChild(receiverElement);

            UnityEngine.Debug.Log("[AdTrace]: AdTrace broadcast receiver successfully added to your app's AndroidManifest.xml file.");

            return true;
        }
    }

    private static void CleanManifestFile(String manifestPath)
    {
        // Due to XML writing issue with XmlElement methods which are unable
        // to write "android:[param]" string, we have wrote "android__[param]" string instead.
        // Now make the replacement: "android:[param]" -> "android__[param]"

        TextReader manifestReader = new StreamReader(manifestPath);
        string manifestContent = manifestReader.ReadToEnd();
        manifestReader.Close();

        Regex regex = new Regex("android__");
        manifestContent = regex.Replace(manifestContent, "android:");

        TextWriter manifestWriter = new StreamWriter(manifestPath);
        manifestWriter.Write(manifestContent);
        manifestWriter.Close();
    }

    private static List<XmlNode> getCustomRecieverNodes(XmlNode applicationNode)
    {
        List<XmlNode> nodes = new List<XmlNode>();
        foreach (XmlNode node in applicationNode.ChildNodes)
        {
            if (node.Name == "receiver")
            {
                foreach (XmlNode subnode in node.ChildNodes)
                {
                    if (subnode.Name == "intent-filter")
                    {
                        foreach (XmlNode subsubnode in subnode.ChildNodes)
                        {
                            if (subsubnode.Name == "action")
                            {
                                foreach (XmlAttribute attribute in subsubnode.Attributes)
                                {
                                    if (attribute.Value.Contains("com.android.vending.INSTALL_REFERRER"))
                                    {
                                        nodes.Add(node);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        return nodes;
    }
}
