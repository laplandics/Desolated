namespace UIElement
{
    public class MainMenuScreenVm : UIElementVm
    {
        public override string BinderKey => "MainMenu";
        
        public void PlayGame()
        {
            var menuSceneConfig = Main.CurrentSceneConfig.Value;
            var exitSubject = menuSceneConfig.ExitSubject;
            var playerConfig = Main.CurrentPlayerConfig.Value;
            var lastPlayerStageConfig = playerConfig.LastVisitedStageConfig.Value;
            exitSubject.OnNext(lastPlayerStageConfig);
        }
    }
}