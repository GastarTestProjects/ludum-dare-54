using _Game.Scripts.Effects;
using _Game.Scripts.Enemies;
using _Game.Scripts.Player;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Installers
{
    [CreateAssetMenu(menuName = "Custom/GameConfigInstaller")]
    public class GameConfigInstaller: ScriptableObjectInstaller<GameConfigInstaller>
    {
        public GameInstaller.Config GameInstaller;
        public PlayerController.Config PlayerController;
        public EnemySpawner.Config EnemySpawner;
        public EnemyExplosion.Config EnemyExplosion;
        public EnemySpawnPoint.Config EnemySpawnPoint;
        public StaminaHandler.Config StaminaHandler;
        public BombSpawner.Config BombSpawner;
        public Bomb.Config Bomb;
        public EnemyController.Config EnemyController;
        public EnemyAnimations.Config EnemyAnimations;
        
        
        public override void InstallBindings()
        {
            Container.BindInstance(GameInstaller).IfNotBound();
            Container.BindInstance(PlayerController).IfNotBound();
            Container.BindInstance(EnemySpawner).IfNotBound();
            Container.BindInstance(EnemyExplosion).IfNotBound();
            Container.BindInstance(EnemySpawnPoint).IfNotBound();
            Container.BindInstance(StaminaHandler).IfNotBound();
            Container.BindInstance(BombSpawner).IfNotBound();
            Container.BindInstance(Bomb).IfNotBound();
            Container.BindInstance(EnemyController).IfNotBound();
            Container.BindInstance(EnemyAnimations).IfNotBound();
        }
    }
}