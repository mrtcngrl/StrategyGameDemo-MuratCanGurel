using Game.Signals.Core;
using Zenject;

namespace Game.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var parentContainer = Container.ParentContainers[0];
            SignalSystem.Initialize(parentContainer);
            GameConstants.Initialize();
        }
    }
}