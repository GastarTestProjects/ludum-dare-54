using UnityEngine;
using Zenject;

namespace _Game.Scripts.Installers
{
    [CreateAssetMenu(menuName = "Custom/GameConfigInstaller")]
    public class GameConfigInstaller: ScriptableObjectInstaller<GameConfigInstaller>
    {
        public GameInstaller.Config GameInstaller;
        
        
        public override void InstallBindings()
        {
            Container.BindInstance(GameInstaller).IfNotBound();
        }
    }
}