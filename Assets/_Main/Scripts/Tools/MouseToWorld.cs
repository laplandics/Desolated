using UnityEngine;
using UnityEngine.InputSystem;

namespace Tools
{
    public static class MouseToWorld
    {
        public static Vector3 WithAbstractPlane()
        {
            return new Vector3(0, 0, 0);
        }

        public static bool WithSurfacePlane(out Vector3 point)
        {
            point = Vector3.zero;
            var mousePos = Mouse.current.position.ReadValue();
            var camera = Main.CurrentMainCamera.Value;
            if (camera == null) camera = Camera.main;
            
            if (camera == null)
            { Debug.LogWarning("Tools.MouseToWorld error: No camera found"); return false; }

            var layerMask = LayerMask.GetMask("Surface");
            var ray = camera.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out var hit, 100, layerMask))
            { point = hit.point; return true; }
            
            Debug.Log("Tools.MouseToWorld: No surface point found");
            return false;
        }
    }
}