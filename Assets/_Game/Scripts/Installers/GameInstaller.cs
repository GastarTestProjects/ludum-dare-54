using System;
using _Game.Scripts.Enemies;
using _Game.Scripts.Enemy;
using _Game.Scripts.Game.Events;
using _Game.Scripts.Game.Models;
using _Game.Scripts.Game.Player;
using UnityEngine.Serialization;
using Zenject;

namespace _Game.Scripts.Installers
{
    public class GameInstaller: MonoInstaller<GameInstaller>
    {
        [Inject]
        private Config _config;
        
        public override void InstallBindings()
        {
            Container.Bind<PlayerInput>().AsSingle();
            Container.Bind<OtherInput>().AsSingle();

            Container.BindFactory<EnemyInitParams, EnemyController, EnemyController.Factory>()
                .FromPoolableMemoryPool<EnemyInitParams, EnemyController, EnemyPool>(poolBinder => poolBinder
                    .WithInitialSize(10)
                    .FromComponentInNewPrefab(_config.enemyControllerPrefab)
                    .UnderTransformGroup("Enemies"));

            InstallEvents();
        }

        private void InstallEvents()
        {
            SignalBusInstaller.Install(Container);
            
            Container.DeclareSignal<PlayerDiedEvent>();
            Container.DeclareSignal<PlayerInitializedEvent>();
            Container.DeclareSignal<PauseEvent>();
        }


        [Serializable]
        public class Config
        {
            [FormerlySerializedAs("enemyPrefab")] public EnemyController enemyControllerPrefab;
        }


        class EnemyPool : MonoPoolableMemoryPool<EnemyInitParams, IMemoryPool, EnemyController>
        {
        }
    }
}