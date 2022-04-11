// Inspired by: https://github.com/facebook/facebook-sdk-for-unity/blob/master/Facebook.Unity.Settings/FacebookSettings.cs


//  Created by Nasser Amini (namini40@github.com) on April 2022.
//  Copyright (c) AdTrace (adtrace.io) . All rights reserved.


using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AdTraceSettings : ScriptableObject
{
    private static AdTraceSettings instance;

    [SerializeField]
    private bool isPostProcessingEnabled = true;
    [SerializeField]
    private bool isiOS14ProcessingEnabled = false;

    public static AdTraceSettings Instance
    {
        get
        {
            instance = NullableInstance;

            if (instance == null)
            {
                // Create AdTraceSettings.asset inside the folder in which AdTraceSettings.cs reside.
                instance = ScriptableObject.CreateInstance<AdTraceSettings>();
                var guids = AssetDatabase.FindAssets(string.Format("{0} t:script", "AdTraceSettings"));
                if (guids == null || guids.Length <= 0)
                {
                    return instance;
                }
                var assetPath = AssetDatabase.GUIDToAssetPath(guids[0]).Replace("AdTraceSettings.cs", "AdTraceSettings.asset");
                AssetDatabase.CreateAsset(instance, assetPath);

                // Before switching to AssetsDatabase, EditorPrefs were used to write 'adtraceiOS14Support' key.
                // Check if this key still exists in EditorPrefs.
                // If yes, migrate the value to AdTraceSettings.asset and remove the key from EditorPrefs.
                if (EditorPrefs.HasKey("adtraceiOS14Support"))
                {
                    UnityEngine.Debug.Log("[AdTrace]: Found 'adtraceiOS14Support' key in EditorPrefs.");
                    UnityEngine.Debug.Log("[AdTrace]: Migrating that value to AdTraceSettings.asset.");
                    IsiOS14ProcessingEnabled = EditorPrefs.GetBool("adtraceiOS14Support", false);
                    EditorPrefs.DeleteKey("adtraceiOS14Support");
                    UnityEngine.Debug.Log("[AdTrace]: Key 'adtraceiOS14Support' removed from EditorPrefs.");
                }
            }

            return instance;
        }
    }

    public static AdTraceSettings NullableInstance
    {
        get
        {
            if (instance == null)
            {
                var guids = AssetDatabase.FindAssets(string.Format("{0} t:ScriptableObject", "AdTraceSettings"));
                if (guids == null || guids.Length <= 0)
                {
                    return instance;
                }
                var assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
                instance = (AdTraceSettings)AssetDatabase.LoadAssetAtPath(assetPath, typeof(AdTraceSettings));
            }

            return instance;
        }
    }

    public static bool IsPostProcessingEnabled
    {
        get
        {
            return Instance.isPostProcessingEnabled;
        }

        set
        {
            if (Instance.isPostProcessingEnabled != value)
            {
                Instance.isPostProcessingEnabled = value;
            }
        }
    }

    public static bool IsiOS14ProcessingEnabled
    {
        get
        {
            return Instance.isiOS14ProcessingEnabled;
        }

        set
        {
            if (Instance.isiOS14ProcessingEnabled != value)
            {
                Instance.isiOS14ProcessingEnabled = value;
            }
        }
    }
}
