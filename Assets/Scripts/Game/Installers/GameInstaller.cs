using Game.Controllers;
using Game.Signals.Core;
using Zenject;

namespace Game.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SelectionController>().AsSingle().NonLazy();
            SignalSystem.Initialize((Container));
            GameConstants.Initialize();
        }
    }
}