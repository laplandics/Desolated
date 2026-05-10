using UnityEngine;
using WorldObject;

namespace Helper.EntityManagement
{
    public class EntityTransformsManager
    {
        private readonly Entity _owner;

        public EntityTransformsManager(Entity owner)
        {
            _owner = owner;
        }
        
        public void ApplyPosition(Vector3 position)
        {
            _owner.transform.position = position;
            _owner.state.currentPosition = position;
        }

        public void ApplyRotation(Vector3 rotation)
        {
            _owner.transform.eulerAngles = rotation;
            _owner.state.currentRotation = rotation;
        }

        public void SetTargetPosition(Vector3 position)
        {
            _owner.state.targetPosition = position;
        }

        public void SetTargetRotation(Vector3 rotation)
        {
            _owner.state.targetRotation = rotation;
        }
    }
}