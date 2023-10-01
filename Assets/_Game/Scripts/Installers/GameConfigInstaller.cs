using _Game.Scripts.Effects;
using _Game.Scripts.Enemies;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Installers
{
    [CreateAssetMenu(menuName = "Custom/GameConfigInstaller")]
    public class GameConfigInstaller: ScriptableObjectInstaller<GameConfigInstaller>
    {
        public GameInstaller.Config GameInstaller;
        public EnemySpawner.Config EnemySpawner;
        public EnemyExplosion.Config EnemyExplosion;
        
        
        public override void InstallBindings()
        {
            Container.BindInstance(GameInstaller).IfNotBound();
            Container.BindInstance(EnemySpawner).IfNotBound();
            Container.BindInstance(EnemyExplosion).IfNotBound();
        }
    }
}