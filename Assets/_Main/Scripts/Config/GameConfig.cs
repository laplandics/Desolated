using Data.Config;
using R3;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Config/Game")]
    public class GameConfig : ConfigBase<GameData>
    {
        public override void Init()
        {
            Main.CurrentEncounterState.Value = data.encounterState;
            Main.CurrentEncounterState.Subscribe(value => data.encounterState = value);
        }
    }
}