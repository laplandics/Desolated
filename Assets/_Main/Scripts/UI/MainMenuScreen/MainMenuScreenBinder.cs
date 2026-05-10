using UnityEngine;
using UnityEngine.UI;

namespace UIElement
{
    public class MainMenuScreenBinder : UIElementBinder<MainMenuScreenVm>
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button exitButton;
        
        protected override void OnBind()
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }

        private void OnPlayButtonClicked() => Vm.PlayGame();

        protected override void OnUnBind()
        {
            playButton.onClick.RemoveListener(OnPlayButtonClicked);
        }
    }
}