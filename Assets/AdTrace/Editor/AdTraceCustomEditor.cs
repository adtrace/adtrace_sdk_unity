using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor;

namespace io.adtrace.sdk
{
    [CustomEditor(typeof(AdTrace))]
    public class AdTraceCustomEditor : Editor
    {
        private Editor settingsEditor;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var adtrace = target as AdTrace;
            GUIStyle darkerCyanTextFieldStyles = new GUIStyle(EditorStyles.boldLabel);
            darkerCyanTextFieldStyles.normal.textColor = new Color(0f/255f, 190f/255f, 190f/255f);

            // Not gonna ask: http://answers.unity.com/answers/1244650/view.html
            EditorGUILayout.Space();
            var origFontStyle = EditorStyles.label.fontStyle;
            EditorStyles.label.fontStyle = FontStyle.Bold;
            adtrace.startManually = EditorGUILayout.Toggle("START SDK MANUALLY", adtrace.startManually, EditorStyles.toggle);
            EditorStyles.label.fontStyle = origFontStyle;
 
            using (new EditorGUI.DisabledScope(adtrace.startManually))
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("MULTIPLATFORM SETTINGS:", darkerCyanTextFieldStyles);
                EditorGUI.indentLevel += 1;
                adtrace.appToken = EditorGUILayout.TextField("App Token", adtrace.appToken);
                adtrace.environment = (AdTraceEnvironment)EditorGUILayout.EnumPopup("Environment", adtrace.environment);
                adtrace.logLevel = (AdTraceLogLevel)EditorGUILayout.EnumPopup("Log Level", adtrace.logLevel);
                adtrace.urlStrategy = (AdTraceUrlStrategy)EditorGUILayout.EnumPopup("URL Strategy", adtrace.urlStrategy);
                adtrace.eventBuffering = EditorGUILayout.Toggle("Event Buffering", adtrace.eventBuffering);
                adtrace.sendInBackground = EditorGUILayout.Toggle("Send In Background", adtrace.sendInBackground);
                adtrace.launchDeferredDeeplink = EditorGUILayout.Toggle("Launch Deferred Deep Link", adtrace.launchDeferredDeeplink);
                adtrace.needsCost = EditorGUILayout.Toggle("Cost Data In Attribution Callback", adtrace.needsCost);
                adtrace.coppaCompliant = EditorGUILayout.Toggle("COPPA Compliant", adtrace.coppaCompliant);
                adtrace.linkMe = EditorGUILayout.Toggle("LinkMe", adtrace.linkMe);
                adtrace.defaultTracker = EditorGUILayout.TextField("Default Tracker", adtrace.defaultTracker);
                adtrace.startDelay = EditorGUILayout.DoubleField("Start Delay", adtrace.startDelay);
                EditorGUILayout.LabelField("App Secret:", EditorStyles.label);
                EditorGUI.indentLevel += 1;
                adtrace.secretId = EditorGUILayout.LongField("Secret ID", adtrace.secretId);
                adtrace.info1 = EditorGUILayout.LongField("Info 1", adtrace.info1);
                adtrace.info2 = EditorGUILayout.LongField("Info 2", adtrace.info2);
                adtrace.info3 = EditorGUILayout.LongField("Info 3", adtrace.info3);
                adtrace.info4 = EditorGUILayout.LongField("Info 4", adtrace.info4);
                EditorGUI.indentLevel -= 2;
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("ANDROID SETTINGS:", darkerCyanTextFieldStyles);
                EditorGUI.indentLevel += 1;
                adtrace.preinstallTracking = EditorGUILayout.Toggle("Preinstall Tracking", adtrace.preinstallTracking);
                adtrace.preinstallFilePath = EditorGUILayout.TextField("Preinstall File Path", adtrace.preinstallFilePath);
                adtrace.playStoreKidsApp = EditorGUILayout.Toggle("Play Store Kids App", adtrace.playStoreKidsApp);
                EditorGUI.indentLevel -= 1;
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("IOS SETTINGS:", darkerCyanTextFieldStyles);
                EditorGUI.indentLevel += 1;
                adtrace.iadInfoReading = EditorGUILayout.Toggle("iAd Info Reading", adtrace.iadInfoReading);
                adtrace.adServicesInfoReading = EditorGUILayout.Toggle("AdServices Info Reading", adtrace.adServicesInfoReading);
                adtrace.idfaInfoReading = EditorGUILayout.Toggle("IDFA Info Reading", adtrace.idfaInfoReading);
                adtrace.skAdNetworkHandling = EditorGUILayout.Toggle("SKAdNetwork Handling", adtrace.skAdNetworkHandling);
                EditorGUI.indentLevel -= 1;
            }

            if (settingsEditor == null)
            {
                settingsEditor = CreateEditor(AdTraceSettings.Instance);
            }

            settingsEditor.OnInspectorGUI();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(adtrace);
                EditorSceneManager.MarkSceneDirty(adtrace.gameObject.scene);
            }
        }
    }
}
