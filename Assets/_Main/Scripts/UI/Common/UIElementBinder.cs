using UnityEngine;

namespace UIElement
{
    public abstract class UIElementBinder<T> : MonoBehaviour, IUIElementBinder where T : UIElementVm
    {
        protected T Vm;
        
        public void Bind(UIElementVm vm) { Vm = (T)vm; OnBind(); }

        protected abstract void OnBind();
        protected abstract void OnUnBind();
        
        public void UnBind() { OnUnBind(); Destroy(gameObject); }
    }
    
    public interface IUIElementBinder
    { public void Bind(UIElementVm vm); public void UnBind(); }
}
