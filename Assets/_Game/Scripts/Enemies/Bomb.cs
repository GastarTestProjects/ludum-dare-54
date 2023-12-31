﻿using System;
using System.Collections;
using _Game.Scripts.Effects;
using _Game.Scripts.Player;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace _Game.Scripts.Enemies
{
    public class Bomb : MonoBehaviour, IPoolable<IMemoryPool>
    {
        [SerializeField] private BombObject bombObject;
        [SerializeField] private GameObject aimSpot;
        
        private Config _config;
        private EnemyExplosion.Factory _explosionFactory;
        private IMemoryPool _pool;

        private Vector3 _defaultAimScale;
        private Vector3 _defaultAimRotation;
        private PlayerController _player;

        [Inject]
        private void Construct(Config config, PlayerController player, EnemyExplosion.Factory explosionFactory)
        {
            _player = player;
            _explosionFactory = explosionFactory;
            _config = config;
            _defaultAimScale = aimSpot.transform.localScale;
            _defaultAimRotation = aimSpot.transform.eulerAngles;
        }
        
        public void OnDespawned()
        {
            _pool = null;
        }

        public void OnSpawned(IMemoryPool pool)
        {
            _pool = pool;
            aimSpot.SetActive(true);
            bombObject.gameObject.SetActive(false);

            var aimSpotTransform = aimSpot.transform;
            aimSpotTransform.localScale = _defaultAimScale;
            aimSpotTransform.rotation = Quaternion.Euler(_defaultAimRotation);
            DOTween.Kill(this);
            AnimateAimSpot();
            StartCoroutine(DropBombAfterDelay());
        }

        private void Update()
        {
            var aimSpotTransform = aimSpot.transform;
            aimSpotTransform.Rotate(Vector3.up, 180 * Time.deltaTime, Space.World);
        }

        private void AnimateAimSpot()
        {
            var sequence = DOTween.Sequence(this);
            var aimTransform = aimSpot.transform;
            // var endRotation = new Vector3(_defaultAimRotation.x, 360, _defaultAimRotation.z);
            // sequence.Append(
            //     aimTransform.DORotate(endRotation, 1f).SetEase(Ease.Linear));
            sequence.Join(aimTransform.DOScale(_defaultAimScale * 1.5f, 0.6f).SetEase(Ease.InOutQuad));
            // sequence.Join(
            //     aimTransform.DOScale(_defaultAimScale * 0.8f, 1f).SetEase(Ease.InOutQuad));
            sequence.SetLoops(-1, LoopType.Yoyo);

        }

        private IEnumerator DropBombAfterDelay()
        {
            yield return new WaitForSeconds(_config.delayBeforeBombDrop);
            bombObject.gameObject.SetActive(true);
            bombObject.transform.position =
                transform.position + new Vector3(0, _config.bombElevation, 0);
            bombObject.GetComponent<Rigidbody>().velocity = new Vector3(0, -30, 0);
            bombObject.SetOnCollisionListener(BombExplode);
        }

        private void BombExplode()
        {
            if (_pool == null) // Sometimes we get two collisions
                return;
            DOTween.Kill(this);
            var explosion = _explosionFactory.Create(new EnemyExplosionParams(false));
            explosion.transform.position = bombObject.transform.position;
            bombObject.gameObject.SetActive(false);
            if (Vector3.Distance(_player.transform.position, bombObject.transform.position) <=
                _config.explosionDamageDistance)
            {
                _player.TakeDamage((int) _config.damage);
            }
            
            _pool.Despawn(this);
        }

        [Serializable]
        public class Config
        {
            public float delayBeforeBombDrop = 2f;
            public float bombElevation = 10f;
            public float explosionDamageDistance = 2.5f;
            public float damage = 20f;
        }
        
        public class Factory : PlaceholderFactory<Bomb>
        {
        }
    }
}