using _Game.Scripts.Enemies;
using System;
using _Game.Scripts.Game.Models;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool isDebug;
        [SerializeField] private float shootCooldown = 0.5f;
        [SerializeField] private float shootForceMultiplier = 10f;
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private float shotDistance = 10;
        [SerializeField] private float shotAngle = 30;
        [SerializeField] private int damage = 5;

        [Header("Refs")]
        [SerializeField] private Transform cauldron;
        [SerializeField] private GameObject ground;
        [SerializeField] private GameObject mouseTargetDebugObj;
        [SerializeField] private Rigidbody playerRigidbody;
        [SerializeField] private Transform playerVisuals;
        [SerializeField] private PlayerWeapon weapon;
        [SerializeField] private PlayerAnimations animations;
        [Header("Sound")]
        [SerializeField] private AudioSource shotAudioSource;
        [SerializeField] private AudioClip shotAudioClip;
        [SerializeField] private AudioSource frictionAudioSource;
        [SerializeField] private AudioClip frictionAudioClip;

        private bool _isDead;
        private int _currentHealth;
        private float _currentShootCooldown;
        private PlayerInput _playerInput;
        [Inject]
        private SignalBus _signalBus;
        private Camera _camera;
        private Plane _targetPlane = new(Vector3.up, 0);
        private Vector3 _surfaceNormal;
        private bool _isGrounded;
        private EnemyRegistry _enemyRegistry;

        [Inject]
        private void Construct(PlayerInput playerInput, OtherInput otherInput, EnemyRegistry enemyRegistry)
        {
            _enemyRegistry = enemyRegistry;
            _playerInput = playerInput;
            _camera = Camera.main;
            animations.PlayIdleAnimation();
            Initialize();
        }

        private void Update()
        {
            CheckHealth();
            ProcessMovement();
            if (_isDead)
            {
                return;
            }

            RotateVisuals();
            var shootCooldownPassed = CheckShootCooldown();
            if (_playerInput.mousePressed && shootCooldownPassed)
                Shoot();
            else
                animations.PlayIdleAnimation();
        }

        private void OnCollisionStay(Collision collision)
        {
            // don't know if we even need this

            var innerIsGrounded = false;
            // foreach (var contact in collision.contacts)
            // {
            //     if (contact.otherCollider.gameObject == ground.gameObject)
            //     {
            //         _surfaceNormal = contact.normal;
            //         innerIsGrounded = true;
            //     }
            // }
            _isGrounded = innerIsGrounded;
        }

        private void Initialize()
        {
            _currentHealth = maxHealth;
            _isDead = false;
            animations.PlayIdleAnimation();
            _signalBus.Fire<PlayerInitializedEvent>();
        }

        private void RotateVisuals()
        {
            var ray = _camera.ScreenPointToRay(_playerInput.mousePosition);
            var worldPosition = Vector3.zero;
            if (_targetPlane.Raycast(ray, out var distance))
                worldPosition = ray.GetPoint(distance);

            if (isDebug)
                mouseTargetDebugObj.transform.position = worldPosition;

            var up = Vector3.up;
            var forward = worldPosition - transform.position;
            forward = Vector3.ProjectOnPlane(forward, Vector3.up);

            if (_isGrounded)
                up = _surfaceNormal;

            var targetRotation = Quaternion.LookRotation(forward, up);
            playerVisuals.rotation = Quaternion.Slerp(playerVisuals.rotation, targetRotation, Time.deltaTime * 10f);
        }

        private void ProcessMovement()
        {
            var currentHorizontalSpeed = Vector3.ProjectOnPlane(playerRigidbody.velocity, Vector3.up).magnitude;
            const float maxVolSpeedThreshold = 6f;
            const float minVolSpeedThreshold = 1f;
            currentHorizontalSpeed = Mathf.Clamp(currentHorizontalSpeed, minVolSpeedThreshold, maxVolSpeedThreshold);
            var volume = currentHorizontalSpeed / (maxVolSpeedThreshold - minVolSpeedThreshold);
            frictionAudioSource.volume = volume;
            if (Math.Abs(currentHorizontalSpeed - minVolSpeedThreshold) < .01f)
            {
                if (frictionAudioSource.isPlaying)
                    frictionAudioSource.Pause();
            }
            else
            {
                if (!frictionAudioSource.isPlaying)
                    frictionAudioSource.Play();
            }
            // Debug.Log($"Horizontal Speed: {currentHorizontalSpeed}");

            // if (currentHorizontalSpeed > .5f)
            // {
            //     if (!frictionAudioSource.isPlaying)
            //         frictionAudioSource.Play();
            // }
            // else
            // {
            //     if (frictionAudioSource.isPlaying)
            //         frictionAudioSource.Pause();
            // }
        }

        private void CheckHealth()
        {
            if (_currentHealth > 0)
                return;
            if (!_isDead)
            {
                _isDead = true;
                _signalBus.Fire<PlayerDiedEvent>();
            }
        }

        private bool CheckShootCooldown()
        {
            if (_currentShootCooldown > 0)
            {
                _currentShootCooldown -= Time.deltaTime;
                return false;
            }

            return true;
        }

        private void Shoot()
        {
            _currentShootCooldown = shootCooldown;
            weapon.Shoot(playerRigidbody, shootForceMultiplier);
            animations.PlayShootAnimation();

            // sound
            var volume = Random.Range(0.9f, 1.1f);
            var pitch = Random.Range(.9f, 1.1f);
            shotAudioSource.pitch = pitch;
            shotAudioSource.PlayOneShot(shotAudioClip, volume);
            
            DamageEnemies();
        }

        private void DamageEnemies()
        {
            for (var i = _enemyRegistry.Enemies.Count - 1; i >= 0; i--)
            {
                var enemy = _enemyRegistry.Enemies[i];
                var distance = Vector3.Distance(enemy.transform.position, transform.position);
                if (distance > shotDistance)
                    continue;
                var angle = Vector3.Angle(
                    enemy.transform.position - transform.position,
                    playerVisuals.forward);
                if (angle > shotAngle)
                    continue;
                enemy.TakeDamage(damage);
            }
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            // _signalBus.Fire(new PlayerTookDamageEvent(_currentHealth));
        }
    }
}