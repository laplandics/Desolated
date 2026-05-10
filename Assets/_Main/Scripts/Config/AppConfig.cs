using Data.Config;
using R3;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "AppConfig", menuName = "Config/App")]
    public class AppConfig : ConfigBase<AppData>
    {
        public ReactiveProperty<int> FPS { get; private set; }
        public ReactiveProperty<int> VSync { get; private set; }

        public override void Init()
        {
            FPS = new ReactiveProperty<int>(data.fps);
            FPS.Skip(1).Subscribe(value => data.fps = value);
            
            VSync = new ReactiveProperty<int>(data.vSync);
            VSync.Skip(1).Subscribe(value => data.vSync = value);
        }
    }
}