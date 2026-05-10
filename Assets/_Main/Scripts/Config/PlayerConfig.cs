using System;
using Constant;
using Data.Config;
using Data.State;
using R3;
using Template;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Config/Player")]
    public class PlayerConfig : ConfigBase<PlayerData>
    {
        public EntityState PlayerEntityState {get; private set;}
        public ReactiveProperty<SceneConfig> LastVisitedStageConfig { get; private set; }
        
        [Space][SerializeField] private EntityTemplate playerEntityTemplate;
        
        protected override void OnRefresh()
        {
            if (string.IsNullOrEmpty(data.lastVisitedStageConfigPath))
            { data.lastVisitedStageConfigPath = Paths.SceneConfigShelterPath; }

            data.playerEntityState = null;
            if (playerEntityTemplate != null)
            {
                data.playerEntityState = playerEntityTemplate.State;
                data.playerEntityState.statuses.Add("PlayerStatus");
                data.playerEntityState.id = "Player: " + Guid.NewGuid();
                data.playerEntityState.key = "Player";
            }
        }

        public override void Init()
        {
            PlayerEntityState = data.playerEntityState;
            
            var lastVisitedStageConfigPath = data.lastVisitedStageConfigPath;
            var lastVisitedStageConfig = R.Get<SceneConfig>(lastVisitedStageConfigPath);
            LastVisitedStageConfig = new ReactiveProperty<SceneConfig>(lastVisitedStageConfig);
            LastVisitedStageConfig.Skip(1).Subscribe(value =>
                data.lastVisitedStageConfigPath = $"Config/Scenes/{value.name}");
        }
    }
}