using System;
using System.Reflection.Emit;
using _Game.Scripts.Enemy;
using _Game.Scripts.Game.Player;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Enemies
{
    public class EnemySpawner: MonoBehaviour
    {
        [SerializeField] private BoxCollider spawnArea;
        
        [Inject]
        private Config _config;
        [Inject]
        private PlayerController _player;
        [Inject]
        private EnemyRegistry _enemyRegistry;
        [Inject]
        private EnemyController.Factory _enemyFactory;

        private float prevSpawnTime;

        private void Update()
        {
            if (Time.time - prevSpawnTime > 1f)
            {
                prevSpawnTime = Time.time;
                if (_enemyRegistry.Enemies.Count < _config.startMaxEnemiesOnScreen)
                {
                    SpawnEnemies();
                }
            }
        }
        
        public void SpawnEnemies()
        {
            for (int i = 0; i < _config.startSpawnAmount; i++)
            {
                var spawnPosition = GetRandomSpawnPosition();
                var enemy = _enemyFactory.Create(
                    new EnemyInitParams(_config.baseHealth, _config.baseSpeed));
                enemy.transform.position = spawnPosition;
            }
        }
        
        // TODO: avoid spawning enemies close to each other
        private Vector3 GetRandomSpawnPosition()
        {
            var spawnPosition = new Vector3(
                UnityEngine.Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
                spawnArea.bounds.min.y,
                UnityEngine.Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z)
            );
            return spawnPosition;
        }
        

        [Serializable]
        public class Config
        {
            public float startMaxEnemiesOnScreen = 8;
            public float startSpawnDelay = 2;
            public float startSpawnAmount = 3;

            [Header("Base enemy stats")]
            public float baseHealth = 1;
            public float baseSpeed = 1;
        }
    }
}