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

        [Header("Refs")]
        [SerializeField] private GameObject mouseTargetDebugObj;
        [SerializeField] private Rigidbody playerRigidbody;
        [SerializeField] private Transform playerVisuals;
        [SerializeField] private PlayerWeapon weapon;
        [SerializeField] private PlayerAnimations animations;
        [Header("Sound")]
        [SerializeField] private AudioSource shotAudioSource;
        [SerializeField] private AudioClip shotAudioClip;
        [SerializeField] private AudioSource frictionAudioSource;

        private Config _config;
        private bool _isDead;
        private int _currentHealth;
        private float _currentShootCooldown;
        private PlayerInput _playerInput;
        [Inject]
        private SignalBus _signalBus;
        private Camera _camera;
        private Plane _targetPlane = new(Vector3.up, 0);
        private EnemyRegistry _enemyRegistry;
        private StaminaHandler _staminaHandler;

        [Inject]
        private void Construct(
            Config config,
            PlayerInput playerInput,
            OtherInput otherInput,
            EnemyRegistry enemyRegistry,
            StaminaHandler staminaHandler
        )
        {
            _config = config;
            _staminaHandler = staminaHandler;
            _enemyRegistry = enemyRegistry;
            _playerInput = playerInput;
            _camera = Camera.main;
            Initialize();
        }

        private void Update()
        {
            PlayMovementSounds();
            CheckHealth();
            if (_isDead)
                return;

            RotateVisuals();
            if (IsShootAllowed())
                Shoot();
            else
                animations.PlayIdleAnimation();
        }

        private void Initialize()
        {
            _currentHealth = _config.MaxHealth;
            _isDead = false;
            animations.SetDefaultRigPointsState();
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

            var targetRotation = Quaternion.LookRotation(forward, up);
            playerVisuals.rotation = Quaternion.Slerp(playerVisuals.rotation, targetRotation, Time.deltaTime * 10f);
        }

        private void PlayMovementSounds()
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

        private bool IsShootAllowed()
        {
            if (_currentShootCooldown > 0)
            {
                _currentShootCooldown -= Time.deltaTime;
                return false;
            }

            return !_staminaHandler.IsHiding && _playerInput.mousePressed && !animations.IsInHiddenState;
        }

        private void Shoot()
        {
            _currentShootCooldown = _config.ShootCooldown;
            weapon.Shoot(playerRigidbody, _config.ShootForceMultiplier);
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
                if (distance > _config.ShotDistance)
                    continue;
                var angle = Vector3.Angle(
                    enemy.transform.position - transform.position,
                    playerVisuals.forward);
                if (angle > _config.ShotAngle)
                    continue;
                enemy.TakeDamage(_config.Damage);
            }
        }


        public void Hide()
        {
            animations.AnimateHide();
            playerRigidbody.velocity *= _config.HideSpeedMultiplier;
        }

        public void Unhide()
        {
            animations.AnimateUnHide();
        }

        public void TakeDamage(int damage)
        {
            if (_staminaHandler.IsHiding)
                return; // TODO: Sound?
            _currentHealth -= damage;
            // _signalBus.Fire(new PlayerTookDamageEvent(_currentHealth));
        }

        [Serializable]
        public class Config
        {
            [field: SerializeField] public float ShootCooldown { get; private set; } = 0.5f;
            [field: SerializeField] public float ShootForceMultiplier { get; private set; } = 1000f;
            [field: SerializeField] public int MaxHealth { get; private set; } = 100;
            [field: SerializeField] public float ShotDistance { get; private set; } = 10;
            [field: SerializeField] public float ShotAngle { get; private set; } = 30;
            [field: SerializeField] public int Damage { get; private set; } = 5;
            [field: SerializeField] public float HideSpeedMultiplier { get; set; } = 1.5f;
        }
    }
}