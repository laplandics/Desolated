using Entity.Ability;
using UnityEngine;

namespace Helpers
{
    public class InteractionCollider : MonoBehaviour
    {
        public Entity.Entity entity;
        public BeInteractable interactable;
        public Collider interactionCollider;
    }
}