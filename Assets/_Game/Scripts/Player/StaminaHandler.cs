using System;
using System.Reflection.Emit;
using _Game.Scripts.Game.Models;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Player
{
    public class StaminaHandler: ITickable
    {
        private Config _config;
        private PlayerInput _playerInput;
        private PlayerController _player;
        
        public float CurrentStamina { get; private set; }
        public bool IsHiding { get; private set; }
        private float _minHideEndTime;
        private float _startRegenTime;

        [Inject]
        private void Construct(Config config, PlayerController player, PlayerInput playerInput)
        {
            _config = config;
            _player = player;
            _playerInput = playerInput;
            CurrentStamina = _config.maxStamina;
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
                CurrentStamina -= _config.costPerSecond * Time.deltaTime;
            else if (CurrentStamina < _config.maxStamina && Time.time >= _startRegenTime)
                CurrentStamina += _config.regenPerSecond * Time.deltaTime;
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