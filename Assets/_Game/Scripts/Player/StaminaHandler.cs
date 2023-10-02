using System;
using _Game.Scripts.Game.Models;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Player
{
    public class StaminaHandler: ITickable
    {
        private Config _config;
        private PlayerInput _playerInput;
        private PlayerController _player;
        
        public float CurrentStamina => _currentStamina.Value;
        public bool IsHiding { get; private set; }
        private float _minHideEndTime;
        private float _startRegenTime;
        private ReactiveProperty<float> _currentStamina = new();

        [Inject]
        private void Construct(Config config, PlayerController player, PlayerHud hud, PlayerInput playerInput)
        {
            _config = config;
            _player = player;
            _playerInput = playerInput;
            _currentStamina.Value = _config.maxStamina;
            hud.SetStaminaMax(_config.maxStamina);
            
            _currentStamina.Subscribe(hud.SetStamina);
        }
        
        public void Tick()
        {
            if (_playerInput.mouse2Pressed && !IsHiding && CurrentStamina > 0)
                StartHiding();
            else if (IsHiding && CurrentStamina <= 0)
                StopHiding();
            else if (!_playerInput.mouse2Pressed && IsHiding && Time.time >= _minHideEndTime)
                StopHiding();
            
            if (IsHiding && CurrentStamina > 0)
                _currentStamina.Value -= _config.costPerSecond * Time.deltaTime;
            else if (CurrentStamina < _config.maxStamina && Time.time >= _startRegenTime)
                _currentStamina.Value += _config.regenPerSecond * Time.deltaTime;
        }
        
        private void StartHiding()
        {
            IsHiding = true;
            _minHideEndTime = Time.time + _config.minDurationAmount;
            _player.Hide();
        }
        
        private void StopHiding()
        {
            IsHiding = false;
            _startRegenTime = Time.time + _config.regenDelay;
            _player.Unhide();
        }

        [Serializable]
        public class Config
        {
            public float maxStamina = 1f;
            public float costPerSecond = 0.5f;
            public float regenPerSecond = 0.2f;
            public float regenDelay = 5f;
            public float minDurationAmount = 0.2f;
        }
    }
}