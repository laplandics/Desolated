using System.IO;
using Constant;
using UnityEditor;
using UnityEngine;

namespace EditorTools.Other
{
    public static class SaveFileCleaner
    {
        private static string Path => Paths.SaveFilePath;
        private static bool _enabled = true;
        
        [MenuItem("Tools/Toggle SaveFileCleaner")]
        private static void ToggleSaveFileCleaner()
        {
            if (_enabled)
            {
                Debug.LogWarning("SaveFileCleaner is disabled");
                _enabled = false;
            }
            else
            {
                Debug.LogWarning("SaveFileCleaner is enabled");
                _enabled = true;
            }
        }
        
        [InitializeOnEnterPlayMode]
        private static void OnSceneLoaded()
        {
            EditorApplication.playModeStateChanged -= OnPlayModStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModStateChanged;
        }

        private static void OnPlayModStateChanged(PlayModeStateChange state)
        {
            if (!_enabled)
            { Debug.LogWarning("Save file hasn't been deleted"); return; }
            
            if (state != PlayModeStateChange.EnteredEditMode) return;
            if (!File.Exists(Path)) return;
            
            File.Delete(Path);
            Debug.Log(Path + " has been deleted.");
        }
    }
}