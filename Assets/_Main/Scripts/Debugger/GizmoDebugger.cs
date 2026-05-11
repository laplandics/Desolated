using System;
using System.Collections.Generic;
using UnityEngine;

namespace DebugTools
{
    public class GizmoDebugger : MonoBehaviour
    {
        public readonly List<Action> DebugActions = new();
        
        public void OnDrawGizmos()
        { foreach (var action in DebugActions) { action?.Invoke(); } }
    }
}
