using System.Linq;
using EntityInteraction;
using UnityEngine;
using UnityEngine.InputSystem;
using WorldObject;

namespace EntitySystem
{
    [CreateAssetMenu(fileName = "PlayerControllerSystem", menuName = "Systems/PlayerController")]
    public class PlayerControllerSystem : SystemBase
    {
        private Inputs _inputs;
        
        protected override void OnEntityRegistered(Entity entity)
        {
            if (RegisteredEntitiesMap.Count > 1)
            { Debug.LogWarning("Multiple Player Entities have been registered"); return; }
            
            _inputs = G.Resolve<Inputs>();
            _inputs.Player.Enable();
            _inputs.Player.Interact.performed += OnPlayerInteract;
        }

        private void OnPlayerInteract(InputAction.CallbackContext ctx)
        {
            if (RegisteredEntitiesMap == null || RegisteredEntitiesMap.Count == 0) return;
            var idEntityPlayer = RegisteredEntitiesMap.First();
            if (!Tools.MouseToWorld.WithSurfacePlane(out var point)) return;
            
            var interaction = new MoveInteraction(idEntityPlayer.Key, point);
            var movementSystem = R.MovementSystem;
            movementSystem.MoveTo(interaction);
        }
    }
}