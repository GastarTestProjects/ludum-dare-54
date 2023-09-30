using _Game.Scripts.Game.Models;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Game.Player
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private OtherInput _otherInput;

        [Inject]
        private void Construct(PlayerInput playerInput, OtherInput otherInput)
        {
            _playerInput = playerInput;
            _otherInput = otherInput;
        }
    }
}