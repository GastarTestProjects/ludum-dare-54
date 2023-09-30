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
        [SerializeField] private GameObject ground;
        [SerializeField] private GameObject mouseTargetDebugObj;
        [SerializeField] private Rigidbody playerRigidbody;
        [SerializeField] private Transform playerVisuals;
        [SerializeField] private PlayerWeapon weapon;
        [SerializeField] private PlayerAnimations animations;

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
            {
                animations.PlayDeathAnimation();
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