using _Game.Scripts.Game.Models;
using _Game.Scripts.Game.Player;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Game
{
    public class InputListener : MonoBehaviour
    {
        private OtherInput _otherInput;
        private PlayerInput _playerInput;

        [Inject]
        private void Construct(PlayerInput playerInput, OtherInput otherInput)
        {
            _playerInput = playerInput;
            _otherInput = otherInput;
        }

        private void Update()
        {
            _otherInput.pausePressed = Input.GetKeyDown(KeyCode.Escape);
            _playerInput.mousePosition = Input.mousePosition;
            _playerInput.mousePressed = Input.GetMouseButton(0);
        }
    }
}