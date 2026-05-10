using System;
using System.Collections.Generic;
using MyAttributes;
using UnityEngine;

namespace Data.State
{
    [Serializable]
    public class EntityState
    {
        [MyReadOnly] public string id;
        [MyReadOnly] public string key;
        [MyReadOnly] public string prefabPath;
        [MyReadOnly] public bool initialized;
        
        [Space]
        
        public int health;
        public float speed;
        
        [Space]
        
        [MyReadOnly] public List<string> systems;
        [MyReadOnly] public List<string> statuses;
        
        [Space]
        
        [MyReadOnly] public Vector3 currentPosition;
        [MyReadOnly] public Vector3 currentRotation;
        
        [Space]
        
        [MyReadOnly] public Vector3 targetPosition;
        [MyReadOnly] public Vector3 targetRotation;
    }
}