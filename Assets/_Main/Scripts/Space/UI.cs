using System;
using System.Collections.Generic;
using System.Linq;
using ObservableCollections;
using R3;
using UIElement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Space
{
    public class UI
    {
        private readonly UIRoot _root;
        
        private readonly ReactiveProperty<UIElementVm> _screen = new();
        private readonly ObservableList<UIElementVm> _windows = new();
        private readonly ObservableList<UIElementVm> _tokens = new();
        
        private readonly Dictionary<UIElementVm, IDisposable> _closeSubscriptionsMap = new();
        
        public ReadOnlyReactiveProperty<UIElementVm> Screen => _screen;
        public IObservableCollection<UIElementVm> Tokens => _tokens;
        public IObservableCollection<UIElementVm> Windows => _windows;
        
        public UI()
        {
            var prefab = R.UiRootPrefab;
            var root = Object.Instantiate(prefab);
            root.name = "[UI]";
            Object.DontDestroyOnLoad(root);
            _root = root;
        }

        public void OpenScreen(UIElementVm screenVm)
        {
            _screen.Value?.OnRemove();
            _screen.Value = screenVm;
            screenVm.OnAdd(_root.screenContainer);
        }

        public void ShowWindow(UIElementVm windowVm)
        {
            if (_windows.Contains(windowVm)) {Debug.LogWarning(ShowSameWindowWarning(windowVm.BinderKey)); return;}
            
            _closeSubscriptionsMap.Add(windowVm, windowVm.OnClose.Subscribe(_ =>
            { DisposeUIElement(windowVm); _windows.Remove(windowVm); }));
            _windows.Add(windowVm);
            windowVm.OnAdd(_root.windowsContainer);
        }
        
        public void AddToken(UIElementVm tokenVm)
        {
            if (_tokens.Contains(tokenVm)) { Debug.LogWarning(AddSameTokenWarning(tokenVm.BinderKey)); return; }
            
            _closeSubscriptionsMap.Add(tokenVm, tokenVm.OnClose.Subscribe(_ =>
            { DisposeUIElement(tokenVm); _tokens.Remove(tokenVm); }));
            _tokens.Add(tokenVm);
            tokenVm.OnAdd(_root.tokensContainer);
        }
        
        public void HideWindow(string binderKey)
        {
            var window = _windows.FirstOrDefault(window => window.BinderKey == binderKey);
            
            if (window == null) return;
            _windows.Remove(window);
            DisposeUIElement(window);
            window.OnRemove();
        }

        public void RemoveToken(string binderKey)
        {
            var token = _tokens.FirstOrDefault(token => token.BinderKey == binderKey);
            
            if (token == null) return;
            _tokens.Remove(token);
            DisposeUIElement(token);
            token.OnRemove();
        }
        
        private void DisposeUIElement(UIElementVm elementVm)
        {
            if (!_closeSubscriptionsMap.Remove(elementVm, out var subscription)) return;
            subscription.Dispose();
        }

        private string AddSameTokenWarning(string key) => $"Trying to open token, that already exists {key}";
        private string ShowSameWindowWarning(string key) => $"Trying to show window, that already exists {key}";
    }
}