using System;
using NaughtyAttributes;
using Service;
using UnityEditor;
using UnityEngine;

namespace Config
{
    [Serializable] public abstract class ConfigData {}

    public abstract class ConfigBase : ScriptableObject
    {
        public abstract ConfigData Data { get; }
        
        public abstract void LoadData(ConfigsProxy proxy);
        public abstract void SaveData(ConfigsProxy proxy);
        public virtual void Init() {}
    }
    
    public abstract class ConfigBase<T> : ConfigBase where T : ConfigData, new()
    {
        [SerializeReference] protected T data;
        public override ConfigData Data => data;

        [Button]
        private void Refresh()
        {
            #if UNITY_EDITOR
            data ??= new T();
            
            OnRefresh();
            
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            #endif
        }

        public override void LoadData(ConfigsProxy proxy)
        { data = (T)proxy.DataMap[name]; OnLoad(); }

        public override void SaveData(ConfigsProxy proxy)
        { proxy.DataMap[name] = data; OnSave(); }
        
        protected virtual void OnRefresh() {}
        protected virtual void OnLoad() {}
        protected virtual void OnSave() {}
    }
}