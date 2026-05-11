using UnityEngine;

namespace EntityInteraction
{
    public struct MoveInteraction
    {
        public readonly string ID;
        public Vector3 Point;

        public MoveInteraction(string id, Vector3 point)
        {
            ID = id;
            Point = point;
        }
    }
}