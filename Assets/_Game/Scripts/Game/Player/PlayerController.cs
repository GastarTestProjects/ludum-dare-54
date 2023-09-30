using _Game.Scripts.Game.Models;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Game.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool isDebug;
        [SerializeField] private float shootCooldown = 0.5f;
        [SerializeField] private float shootForceMultiplier = 10f;
        [SerializeField] private int maxHealth = 100;

        [Header("Refs")]
        [SerializeField] private GameObject lookAtDebugObj;
        [SerializeField] private Rigidbody playerRigidbody;
        [SerializeField] private Transform playerHumanoid3d;
        [SerializeField] private PlayerAnimations animations;
        [SerializeField] private PlayerWeapon weapon;
        private bool _isDead;
        private int _currentHealth;
        private float _currentShootCooldown;
        private PlayerInput _playerInput;
        [Inject]
        private SignalBus _signalBus;
        private Camera _camera;
        private Plane _plane = new(Vector3.up, 0);

        [Inject]
        private void Construct(PlayerInput playerInput, OtherInput otherInput)
        {
            _playerInput = playerInput;
            _camera = Camera.main;
            animations.PlayIdleAnimation();
            Initialize();
        }

        private void Update()
        {
            CheckHealth();
            if (_isDead)
                return;

            RotateHumanoid();
            var shootCooldownPassed = CheckShootCooldown();
            if (_playerInput.mousePressed && shootCooldownPassed)
                Shoot();
            else
                animations.PlayIdleAnimation();
        }

        private void Initialize()
        {
            _currentHealth = maxHealth;
            _isDead = false;
            animations.PlayIdleAnimation();
            _signalBus.Fire<PlayerInitializedEvent>();
        }

        private void RotateHumanoid()
        {
            var ray = _camera.ScreenPointToRay(_playerInput.mousePosition);
            var worldPosition = Vector3.zero;
            if (_plane.Raycast(ray, out var distance))
                worldPosition = ray.GetPoint(distance);

            if (isDebug)
                lookAtDebugObj.transform.position = worldPosition;

            // local humanoid3d is rotated to look at worldPosition
            var targetRotation = Quaternion.LookRotation(worldPosition - playerHumanoid3d.position);
            // no rotation in x and z axis
            targetRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);


            playerHumanoid3d.rotation = Quaternion.Slerp(
                playerHumanoid3d.rotation,
                targetRotation,
                10 * Time.deltaTime);
        }

        private void CheckHealth()
        {
            if (_currentHealth > 0)
                return;
            _isDead = true;
            _signalBus.Fire<PlayerDiedEvent>();
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
        }
    }
}