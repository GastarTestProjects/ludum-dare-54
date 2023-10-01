using System;
using _Game.Scripts.Effects;
using _Game.Scripts.Enemies;
using _Game.Scripts.Enemy;
using _Game.Scripts.Game.Events;
using _Game.Scripts.Game.Models;
using _Game.Scripts.Player;
using UnityEngine;
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
            Container.Bind<EnemyRegistry>().AsSingle();

            Container.BindFactory<EnemyInitParams, EnemyController, EnemyController.Factory>()
                .FromPoolableMemoryPool<EnemyInitParams, EnemyController, EnemyPool>(poolBinder => poolBinder
                    .WithInitialSize(10)
                    .FromComponentInNewPrefab(_config.enemyPrefab)
                    .UnderTransformGroup("Enemies"));

            Container.BindFactory<EnemyExplosionParams, EnemyExplosion, EnemyExplosion.Factory>()
                .FromPoolableMemoryPool<EnemyExplosionParams, EnemyExplosion, EnemyExplosionPool>(
                    poolBinder => poolBinder
                        .WithInitialSize(5)
                        .FromComponentInNewPrefab(_config.enemyExplosionPrefab)
                        .UnderTransformGroup("EnemyExplosions"));
            
            Container.BindFactory<EnemySpawnPoint, EnemySpawnPoint.Factory>()
                .FromPoolableMemoryPool<EnemySpawnPoint, EnemySpawnPointPool>(
                    poolBinder => poolBinder
                        .WithInitialSize(5)
                        .FromComponentInNewPrefab(_config.enemySpawnPointPrefab)
                        .UnderTransformGroup("EnemySpawnPoints"));

            InstallEvents();
        }

        private void InstallEvents()
        {
            SignalBusInstaller.Install(Container);
            
            Container.DeclareSignal<PlayerDiedEvent>();
            Container.DeclareSignal<PlayerInitializedEvent>().OptionalSubscriber();
            Container.DeclareSignal<PauseEvent>();
        }


        [Serializable]
        public class Config
        {
            public GameObject enemyPrefab;
            public GameObject enemyExplosionPrefab;
            public GameObject enemySpawnPointPrefab;
        }


        class EnemyPool : MonoPoolableMemoryPool<EnemyInitParams, IMemoryPool, EnemyController>
        {
        }
        
        class EnemyExplosionPool : MonoPoolableMemoryPool<EnemyExplosionParams, IMemoryPool, EnemyExplosion>
        {
        }
        
        class EnemySpawnPointPool : MonoPoolableMemoryPool<IMemoryPool, EnemySpawnPoint>
        {
        }
    }
}