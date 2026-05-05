using System.IO;
using Data;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace EditorTools
{
    public abstract class BaseConverter : ScriptableObject
    {
        private static string Folder => "Assets/_Main/Resources/Json/";
        protected abstract string Label { get; }
        
        [Button]
        public void Convert()
        {
            Tools.JsonSettingsSetter.SetSettings();
            
            var state = GetState();
            var json = JsonConvert.SerializeObject(state, Formatting.Indented);
            var path = Folder + Label + ".json";
            
            File.WriteAllText(path, json);
            AssetDatabase.ImportAsset(path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        public abstract DataState GetState();
    }
}