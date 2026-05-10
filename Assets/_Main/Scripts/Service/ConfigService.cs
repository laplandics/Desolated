using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Config;
using Constant;
using Newtonsoft.Json;
using UnityEngine;

namespace Service
{
    [Serializable] public class ConfigsProxy { public Dictionary<string, ConfigData> DataMap = new(); }
    public class ConfigService : ServiceBase, IOnProjectBeginLoadService, IOnSceneBeginUnloadService
    {
        private readonly List<ConfigBase> _configs = new();

        public ConfigService() => Tools.JsonSettingsSetter.SetSettings();
        
        public IEnumerator OnProjectBeginLoad()
        {
            _configs.Clear();
            var configs = R.GetAll<ConfigBase>("Config");
            _configs.AddRange(configs);

            var path = Paths.SaveFilePath;
            if (File.Exists(path))
            {
                var task = File.ReadAllTextAsync(path);
                yield return new WaitUntil(() => task.IsCompleted);
                var proxy = JsonConvert.DeserializeObject<ConfigsProxy>(task.Result);
                foreach (var config in configs) config.LoadData(proxy);
            }
            
            foreach (var config in _configs) config.Init();
            yield return null;
        }

        public IEnumerator OnSceneBeginUnload()
        {
            var path = Paths.SaveFilePath;
            var proxy = new ConfigsProxy();

            foreach (var config in _configs)
            { config.SaveData(proxy); }
            
            var json = JsonConvert.SerializeObject(proxy, Formatting.Indented);
            var task = File.WriteAllTextAsync(path, json);
            yield return new WaitUntil(() => task.IsCompleted);
        }
    }
}