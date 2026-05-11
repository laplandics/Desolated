using System;
using System.Collections.Generic;
using Data.Config;
using Data.State;
using ObservableCollections;
using R3;
using UIElement;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using WorldObject;

namespace Config
{
    [CreateAssetMenu(fileName = "SceneConfig_[SCENE_NAME]", menuName = "Config/Scene")]
    public class SceneConfig : ConfigBase<SceneData>
    {
        public string SceneName => name.Split('_')[1];
        
        public Subject<SceneConfig> ExitSubject;
        public ReactiveProperty<UIElementVm> ScreenVm { get; private set; }
        public ReactiveProperty<CameraConfig> SceneCamera { get; private set; }
        public ObservableList<EntityState> Entities { get; private set; }
        public ReactiveProperty<ScenePlayerInfo> PlayerInfo { get; private set; }

        protected override void OnRefresh()
        {
            if (SceneManager.GetActiveScene().name != SceneName)
            {
                var sceneGuid = AssetDatabase.FindAssets(SceneName, new[] { "Assets/_Main/Scenes" });
                var scenePath = AssetDatabase.GUIDToAssetPath(sceneGuid[0]);
                EditorSceneManager.SaveOpenScenes();
                EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            }
            
            data.entities = new List<EntityState>();
            var entities = FindObjectsByType<Entity>(FindObjectsInactive.Include);
            foreach (var entity in entities) data.entities.Add(entity.GenerateState());
        }

        public override void Init()
        {
            SetScreen();
            SetCamera();
            SetPlayerInfo();
            SetEntities();
        }

        private void SetScreen()
        {
            if (string.IsNullOrEmpty(data.sceneScreen)) return;
            var screenName = data.sceneScreen;
            var screenType = Tools.ReflectionTool.GetTypeByName(screenName);
            if (screenType == null) return;
            var screenVm = Activator.CreateInstance(screenType) as UIElementVm;
            ScreenVm = new ReactiveProperty<UIElementVm>(screenVm);
            ScreenVm.Skip(1).Subscribe(value => data.sceneScreen = value.GetType().Name);
        }

        private void SetCamera()
        {
            if (string.IsNullOrEmpty(data.sceneCameraConfigPath)) return;
            var config = R.Get<CameraConfig>(data.sceneCameraConfigPath);
            if (config == null) return;
            SceneCamera = new ReactiveProperty<CameraConfig>(config);
            SceneCamera.Skip(1).Subscribe(value => data.sceneCameraConfigPath = $"Config/Cameras/{value.name}");
        }

        private void SetPlayerInfo()
        {
            var info = data.playerInfo;
            PlayerInfo = new ReactiveProperty<ScenePlayerInfo>(info);
            PlayerInfo.Skip(1).Subscribe(value => data.playerInfo = value);
        }

        private void SetEntities()
        {
            Entities = new ObservableList<EntityState>();
            data.entities.ForEach(entity => Entities.Add(entity));
            Entities.ObserveAdd().Subscribe(e => data.entities.Add(e.Value));
            Entities.ObserveRemove().Subscribe(e => data.entities.Remove(e.Value));
        }
    }
}