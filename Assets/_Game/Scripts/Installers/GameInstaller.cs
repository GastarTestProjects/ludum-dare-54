using _Game.Scripts.Game.Models;
using _Game.Scripts.Game.Player;
using Zenject;

namespace _Game.Scripts.Installers
{
    public class GameInstaller: MonoInstaller<GameInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerInput>().AsSingle();
            Container.Bind<OtherInput>().AsSingle();
            
            InstallEvents();
        }

        private void InstallEvents()
        {
            SignalBusInstaller.Install(Container);
            
            Container.DeclareSignal<PlayerDiedEvent>();
        }
    }
}