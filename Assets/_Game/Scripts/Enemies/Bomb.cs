using System;
using System.Collections;
using _Game.Scripts.Effects;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace _Game.Scripts.Enemies
{
    public class Bomb : MonoBehaviour, IPoolable<IMemoryPool>
    {
        [SerializeField] private BombObject bombObject;
        [SerializeField] private DecalProjector aimSpot;
        
        private Config _config;
        private EnemyExplosion.Factory _explosionFactory;
        private IMemoryPool _pool;

        private void Construct(Config config, EnemyExplosion.Factory explosionFactory)
        {
            _explosionFactory = explosionFactory;
            _config = config;
        }
        
        public void OnDespawned()
        {
            _pool = null;
        }

        public void OnSpawned(IMemoryPool pool)
        {
            _pool = pool;
            aimSpot.enabled = true;
            StartCoroutine(DropBombAfterDelay());
        }

        private IEnumerator DropBombAfterDelay()
        {
            yield return new WaitForSeconds(_config.delayBeforeBombDrop);
            bombObject.gameObject.SetActive(true);
            bombObject.transform.position =
                transform.position + new Vector3(0, _config.bombElevation, 0);
            bombObject.SetOnCollisionListener(BombExplode);
        }

        private void BombExplode()
        {
            var explosion = _explosionFactory.Create(new EnemyExplosionParams(false));
            explosion.transform.position = bombObject.transform.position;
            bombObject.gameObject.SetActive(false);
            _pool.Despawn(this);
        }

        [Serializable]
        public class Config
        {
            public float delayBeforeBombDrop = 2f;
            public float bombElevation = 10f;
        }
        
        public class Factory : PlaceholderFactory<Bomb>
        {
        }
    }
}