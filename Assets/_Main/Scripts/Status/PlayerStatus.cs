using System;
using Constant;

namespace EntityStatus
{
    [Serializable]
    public class PlayerStatus : StatusBase
    {
        protected override void OnActivated()
        {
            var path = Paths.PlayerControllerSystemPath;
            if (Owner.SystemsManager.SystemsMap.ContainsKey(path)) return;
            Owner.SystemsManager.AddSystem(path);
        }
    }
}