using UnityEngine;

namespace DebugTools
{
    public class Debugger
    {
        public readonly GizmoDebugger DebugGizmo;
        
        public Debugger()
        {
            DebugGizmo = new GameObject("[GIZMO_DEBUG]")
                .AddComponent<GizmoDebugger>();
            Object.DontDestroyOnLoad(DebugGizmo.gameObject);
        }
    }
}