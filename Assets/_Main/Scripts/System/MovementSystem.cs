using Constant;
using UnityEngine;

namespace EntitySystem
{
    [CreateAssetMenu(fileName = "MovementSystem", menuName = "Systems/Movement")]
    public class MovementSystem : SystemBase
    {
        public bool OrderToMove(MoveOrder order)
        {
            var entity = RegisteredEntitiesMap[order.ID];
            var speed = entity.state.speed;
            var currentPosition = entity.state.currentPosition;
            var destination = CalculateDestination(order.Point, speed, currentPosition);
            entity.TransformsManager.SetTargetPosition(destination);
            return true;
        }

        private Vector3 CalculateDestination(Vector3 point, float speed, Vector3 currentPos)
        {
            const float t = Values.TURN_DURATION;
            var distance = speed * t;
            var direction = (currentPos - point).normalized;
            
            var destination = currentPos + direction * distance;
            return destination;
        }
    }

    public struct MoveOrder
    {
        public readonly string ID;
        public Vector3 Point;
        
        public MoveOrder(string id, Vector3 point)
        {
            ID = id;
            Point = point;
        }
    }
}