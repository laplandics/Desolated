using System;
using R3;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UIElement
{
    public abstract class UIElementVm
    {
        private readonly Subject<Unit> _onClose = new();
        
        public abstract string BinderKey { get; }
        public IUIElementBinder Binder { get; private set; }
        public Observable<Unit> OnClose => _onClose;
        
        public void InvokeClose() => _onClose.OnNext(Unit.Default);
        
        public void OnAdd(RectTransform root)
        {
            var prefab = R.Get<GameObject>($"Prefab/UI/{BinderKey}");
            if (prefab == null) throw new Exception($"Can't find prefab {BinderKey}");
            var binderObj = Object.Instantiate(prefab, root, false);
            var binder = binderObj.GetComponent<IUIElementBinder>();
            Binder = binder ?? throw new Exception($"Can't find IUIElementBinder {BinderKey}");
            Binder.Bind(this);
        }

        public virtual void OnRemove() { Binder.UnBind(); Binder = null; }
    }
}