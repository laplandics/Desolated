using System;
using Constant;
using Data.Config;
using Data.State;
using R3;
using Template;
using UnityEditor;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Config/Player")]
    public class PlayerConfig : ConfigBase<PlayerData>
    {
        public EntityState PlayerEntityState => data.playerEntityState;
        public ReactiveProperty<SceneConfig> LastVisitedStageConfig { get; private set; }
        
        [Space][SerializeField] private EntityTemplate playerEntityTemplate;
        
        protected override void OnRefresh()
        {
            if (string.IsNullOrEmpty(data.lastVisitedStageConfigPath))
            { data.lastVisitedStageConfigPath = Paths.SceneConfigShelterPath; }
            var playerController = R.PlayerControllerSystem;
            var playerControllerFullPath = AssetDatabase.GetAssetPath(playerController);
            var playerControllerPath = Tools.PathHelper.AssetPathToResourcePath(playerControllerFullPath);
            
            data.playerEntityState = null;
            if (playerEntityTemplate != null)
            {
                data.playerEntityState = playerEntityTemplate.State;
                data.playerEntityState.statuses.Add("PlayerStatus");
                data.playerEntityState.systems.Add(playerControllerPath);
                data.playerEntityState.id = "Player: " + Guid.NewGuid();
                data.playerEntityState.key = "Player";
            }
        }

        public override void Init()
        {
            var lastVisitedStageConfigPath = data.lastVisitedStageConfigPath;
            var lastVisitedStageConfig = R.Get<SceneConfig>(lastVisitedStageConfigPath);
            LastVisitedStageConfig = new ReactiveProperty<SceneConfig>(lastVisitedStageConfig);
            LastVisitedStageConfig.Skip(1).Subscribe(value =>
                data.lastVisitedStageConfigPath = $"Config/Scenes/{value.name}");
        }
    }
}