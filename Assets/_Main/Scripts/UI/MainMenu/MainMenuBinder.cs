using UnityEngine;
using UnityEngine.UI;

namespace UIElement
{
    public class MainMenuBinder : UIElementBinder<MainMenuVm>
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button exitButton;
        
        protected override void OnBind()
        {
            
        }

        protected override void OnUnBind()
        {
            
        }
    }
}