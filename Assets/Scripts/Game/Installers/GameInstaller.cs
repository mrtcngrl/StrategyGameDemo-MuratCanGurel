using Game.Controllers;
using Zenject;

namespace Game.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SelectionController>().AsSingle().NonLazy();
            GameConstants.Initialize();
        }
    }
}