using _Game.Scripts.Game.Models;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Game
{
    public class InputListener : MonoBehaviour
    {
        private FullInput _fullInput;

        [Inject]
        private void Construct(FullInput fullInput)
        {
            _fullInput = fullInput;
        }

        private void Update()
        {
            _fullInput.pausePressed = Input.GetKeyDown(KeyCode.Escape);
        }
    }
}