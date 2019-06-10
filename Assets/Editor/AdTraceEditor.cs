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
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

public class AdTraceEditor
{
    private static bool isPostProcessingEnabled = true;

    [MenuItem("Assets/AdTrace/Check post processing status")]
    public static void CheckPostProcessingPermission()
    {
        EditorUtility.DisplayDialog("[AdTrace]", "The post processing for AdTrace SDK is " + (isPostProcessingEnabled ? "enabled." : "disabled."), "OK");
    }

    [MenuItem("Assets/AdTrace/Change post processing status")]
    public static void ChangePostProcessingPermission()
    {
        isPostProcessingEnabled = !isPostProcessingEnabled;
        EditorUtility.DisplayDialog("[AdTrace]", "The post processing for AdTrace SDK is now " + (isPostProcessingEnabled ? "enabled." : "disabled."), "OK");
    }

    [MenuItem("Assets/AdTrace/Export Unity Package")]
    static void ExportAdTraceUnityPackage()
    {
        string exportedFileName = "AdTrace.unitypackage";
        string assetsPath = "Assets/AdTrace";
        List<string> assetsToExport = new List<string>();

        // AdTrace Editor script.
        assetsToExport.Add("Assets/Editor/AdTraceEditor.cs");
        // AdTrace Assets.
        assetsToExport.Add(assetsPath + "/AdTrace.cs");
        assetsToExport.Add(assetsPath + "/AdTrace.prefab");
        assetsToExport.Add(assetsPath + "/3rd Party/SimpleJSON.cs");

        assetsToExport.Add(assetsPath + "/Android/adtrace-android.jar");
        assetsToExport.Add(assetsPath + "/Android/AdTraceAndroid.cs");
        assetsToExport.Add(assetsPath + "/Android/AdTraceAndroidManifest.xml");

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
        assetsToExport.Add(assetsPath + "/iOS/AdTrace.h");
        assetsToExport.Add(assetsPath + "/iOS/AdTraceiOS.cs");
        assetsToExport.Add(assetsPath + "/iOS/AdTraceSdk.a");
        assetsToExport.Add(assetsPath + "/iOS/AdTraceUnity.h");
        assetsToExport.Add(assetsPath + "/iOS/AdTraceUnity.mm");
        assetsToExport.Add(assetsPath + "/iOS/AdTraceUnityDelegate.h");
        assetsToExport.Add(assetsPath + "/iOS/AdTraceUnityDelegate.mm");

        assetsToExport.Add(assetsPath + "/Unity/AdTraceAttribution.cs");
        assetsToExport.Add(assetsPath + "/Unity/AdTraceConfig.cs");
        assetsToExport.Add(assetsPath + "/Unity/AdTraceEnvironment.cs");
        assetsToExport.Add(assetsPath + "/Unity/AdTraceEvent.cs");
        assetsToExport.Add(assetsPath + "/Unity/AdTraceEventFailure.cs");
        assetsToExport.Add(assetsPath + "/Unity/AdTraceEventSuccess.cs");
        assetsToExport.Add(assetsPath + "/Unity/AdTraceLogLevel.cs");
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
        if (!isPostProcessingEnabled)
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
        else if (target == BuildTarget.iOS)
        {
#if UNITY_IOS
            UnityEngine.Debug.Log("[AdTrace]: Starting to perform post build tasks for iOS platform.");
            
            string xcodeProjectPath = projectPath + "/Unity-iPhone.xcodeproj/project.pbxproj";

            PBXProject xcodeProject = new PBXProject();
            xcodeProject.ReadFromFile(xcodeProjectPath);

            // The AdTrace SDK needs two frameworks to be added to the project:
            // - AdSupport.framework
            // - iAd.framework

            string xcodeTarget = xcodeProject.TargetGuidByName("Unity-iPhone");
            
            UnityEngine.Debug.Log("[AdTrace]: Adding AdSupport.framework to Xcode project.");
            xcodeProject.AddFrameworkToProject(xcodeTarget, "AdSupport.framework", true);
            UnityEngine.Debug.Log("[AdTrace]: AdSupport.framework added successfully.");

            UnityEngine.Debug.Log("[AdTrace]: Adding iAd.framework to Xcode project.");
            xcodeProject.AddFrameworkToProject(xcodeTarget, "iAd.framework", true);
            UnityEngine.Debug.Log("[AdTrace]: iAd.framework added successfully.");

            UnityEngine.Debug.Log("[AdTrace]: Adding CoreTelephony.framework to Xcode project.");
            xcodeProject.AddFrameworkToProject(xcodeTarget, "CoreTelephony.framework", true);
            UnityEngine.Debug.Log("[AdTrace]: CoreTelephony.framework added successfully.");

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

            // Save the changes to Xcode project file.
            xcodeProject.WriteToFile(xcodeProjectPath);
#endif
        }
    }

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
            
            // Add needed permissions if they are missing.
            AddPermissions(manifestFile);

            // Add intent filter to main activity if it is missing.
            AddBroadcastReceiver(manifestFile);

            // Save the changes.
            manifestFile.Save(appManifestPath);

            // Clean the manifest file.
            CleanManifestFile(appManifestPath);

            UnityEngine.Debug.Log("[AdTrace]: App's AndroidManifest.xml file check and potential modification completed.");
            UnityEngine.Debug.Log("[AdTrace]: Please check if any error message was displayed during this process " 
                + "and make sure to fix all issues in order to properly use the AdTrace SDK in your app.");
        }
    }

    private static void AddPermissions(XmlDocument manifest)
    {
        // The AdTrace SDK needs two permissions to be added to you app's manifest file:
        // <uses-permission android:name="android.permission.INTERNET" />
        // <uses-permission android:name="android.permission.ACCESS_WIFI_STATE" />
        // <uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
        // <uses-permission android:name="com.google.android.finsky.permission.BIND_GET_INSTALL_REFERRER_SERVICE" />

        UnityEngine.Debug.Log("[AdTrace]: Checking if all permissions needed for the AdTrace SDK are present in the app's AndroidManifest.xml file.");

        bool hasInternetPermission = false;
        bool hasAccessWifiStatePermission = false;
        bool hasAccessNetworkStatePermission = false;
        bool hasInstallReferrerServicePermission = false;

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
                }
            }
        }

        // If android.permission.INTERNET permission is missing, add it.
        if (!hasInternetPermission)
        {
            XmlElement element = manifest.CreateElement("uses-permission");
            element.SetAttribute("android__name", "android.permission.INTERNET");
            manifestRoot.AppendChild(element);
            UnityEngine.Debug.Log("[AdTrace]: android.permission.INTERNET permission successfully added to your app's AndroidManifest.xml file.");
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
        }
        else
        {
            UnityEngine.Debug.Log("[AdTrace]: Your app's AndroidManifest.xml file already contains com.google.android.finsky.permission.BIND_GET_INSTALL_REFERRER_SERVICE permission.");
        }
    }

    private static void AddBroadcastReceiver(XmlDocument manifest)
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
            return;
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
