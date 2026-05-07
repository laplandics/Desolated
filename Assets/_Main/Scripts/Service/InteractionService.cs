using System.Collections;
using Entity.Ability;
using Helpers;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Service
{
    public class InteractionService : BaseService, IOnSceneEndBootService, IOnSceneBeginUnloadService
    {
        private Inputs _inputs;
        private Coroutine _interactionsCoroutine;
        private bool _interactionsEnabled;
        private Camera _mainCamera;
        private CompositeDisposable _disposables = new();
        
        private (Collider col, BeInteractable inter) _last;
        
        private static int Layer => LayerMask.GetMask("Entity"); 
        
        public IEnumerator OnSceneEndBoot()
        {
            _inputs = G.Resolve<Inputs>();
            _inputs.Player.Interact.Enable();
            _interactionsCoroutine = G.Resolve<Coroutines>()
                .Start(InteractionsCoroutine());
            _interactionsEnabled = true;

            _mainCamera = Main.CurrentMainCamera.Value;
            _disposables.Add(Main.CurrentMainCamera.Subscribe(cam => _mainCamera = cam));
            
            yield return null;
        }

        private IEnumerator InteractionsCoroutine()
        {
            var wait = new WaitUntil(() => _interactionsEnabled);
            while (true)
            {
                yield return wait;
                var mousePos = Mouse.current.position.ReadValue();
                var cam = _mainCamera;
                var ray = cam.ScreenPointToRay(mousePos);
                if (!Physics.Raycast(ray, out var hit, Layer))
                { yield return null; continue;}
                
                var hitCollider = hit.collider;
                if (hitCollider == _last.col)
                { _last.inter.ShowInteraction(); yield return null; continue; }
                
                if (!hitCollider.TryGetComponent<InteractionCollider>(out var helper))
                { yield return null; continue;}

                var interactable = helper.interactable;
                if (interactable == null) { yield return null; continue;}
                
                interactable.ShowInteraction();
                _last.col = hitCollider;
                _last.inter = interactable;
                yield return null;
            }
        }

        public IEnumerator OnSceneBeginUnload()
        {
            _interactionsEnabled = false;
            if (_interactionsCoroutine != null)
            { G.Resolve<Coroutines>().Stop(_interactionsCoroutine); }
            _inputs.Player.Interact.Disable();
            _disposables?.Dispose();
            yield return null;
        }
    }
}