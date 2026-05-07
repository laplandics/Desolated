using System;
using Constant;
using Helpers;
using UnityEngine;

namespace Entity.Ability
{
    public class BeInteractable : BaseModule
    {
        public BeInteractableState state;
        public InteractionCollider interactionCollider;
        
        public override EntityModules ModuleType => EntityModules.BeInteractable;

        public override void OnAdd()
        {
            if (gameObject.layer != LayerMask.NameToLayer("Entity"))
            { Debug.LogWarning(WrongLayerWarning); return; }
            if (interactionCollider == null)
            { Debug.LogWarning(ColliderIsNullWarning); return;}
        }

        public void ShowInteraction()
        {
            Debug.Log($"Interact with {Owner.name}");
        }
        
        public override string ParseToString() => GetState(state);
        public override void ParseToState(string stateString) => state = GetState<BeInteractableState>(stateString);

        private string WrongLayerWarning => $"{Owner.name} can't be interactable as it isn't on the 'Entity' layer";
        private string ColliderIsNullWarning => $"{Owner.name} can't be interactable as it has no collider";
    }

    [Serializable]
    public struct BeInteractableState
    {
        
    }
}