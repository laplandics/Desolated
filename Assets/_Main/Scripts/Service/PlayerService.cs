using System.Collections;
using System.Linq;
using Data.State;
using Utils;
using WorldObject;

namespace Service
{
    public class PlayerService : ServiceBase, IOnSceneEndBootService, IOnSceneBeginUnloadService
    {
        private Entity _playerEntity;
        
        public IEnumerator OnSceneEndBoot()
        {
            var scene = Main.CurrentSceneConfig.Value;
            var player = Main.CurrentPlayerConfig.Value;
            if (scene == player.LastVisitedStageConfig.Value)
            {
                var playerEState = player.PlayerEntityState;
                var scenePlayerEState = scene.Entities.FirstOrDefault(e => e.id == playerEState.id);
                if (scenePlayerEState == null) SpawnPlayer(playerEState);
            }
            
            yield return null;
        }

        private void SpawnPlayer(EntityState playerEState)
        { G.Resolve<Entities>().Builder.New(playerEState).Build(out _playerEntity); }

        public IEnumerator OnSceneBeginUnload()
        {
            DespawnPlayer();
            yield return null;
        }

        private void DespawnPlayer()
        {
            if (_playerEntity == null) return;
            G.Resolve<Entities>().Builder.DestroyEntity(_playerEntity);
        }
    }
}