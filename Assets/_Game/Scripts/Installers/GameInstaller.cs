using _Game.Scripts.Game.Models;
using Zenject;

namespace _Game.Scripts.Installers
{
    public class GameInstaller: MonoInstaller<GameInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerInput>().AsSingle();
            Container.Bind<OtherInput>().AsSingle();
        }
    }
}