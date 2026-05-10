using System.Collections;
using Config;
using Utils;
using WorldObject;

namespace Service
{
    public class PlayerService : ServiceBase, IOnSceneEndBootService, IOnSceneBeginUnloadService
    {
        private Entity _playerEntity;
        
        public IEnumerator OnSceneEndBoot()
        {
            var sceneConfig = Main.CurrentSceneConfig.Value;
            var lastVisitedSceneConfig = Main.CurrentPlayerConfig
                .Value.LastVisitedStageConfig.Value;

            if (sceneConfig == lastVisitedSceneConfig)
            { SpawnPlayer(sceneConfig); }
            
            yield return null;
        }

        public IEnumerator OnSceneBeginUnload()
        {
            if (_playerEntity != null)
            { DespawnPlayer(); }
            
            yield return null;
        }

        private void SpawnPlayer(SceneConfig currentScene)
        {
            var playerConfig = Main.CurrentPlayerConfig.Value;
            var playerEState = playerConfig.PlayerEntityState;
            if (!playerEState.initialized)
            {
                var playerInfo = currentScene.PlayerInfo.Value;
                playerEState.currentPosition = playerInfo.initialPosition;
                playerEState.currentRotation = playerInfo.initialRotation;
            }
            
            G.Resolve<Entities>().Builder.New(playerEState).Build(out _playerEntity);
        }

        private void DespawnPlayer() => G.Resolve<Entities>().Builder.DestroyEntity(_playerEntity);
    }
}