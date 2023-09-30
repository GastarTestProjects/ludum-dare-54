using _Game.Scripts.Game.Models;
using _Game.Scripts.Game.Player;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Game
{
    public class GameController : MonoBehaviour
    {
        private OtherInput _otherInput;

        [Inject]
        private void Construct(OtherInput otherInput, SignalBus signalBus)
        {
            _otherInput = otherInput;
            signalBus.Subscribe<PlayerDiedEvent>(OnPlayerDied);
        }
        
        private void OnPlayerDied(PlayerDiedEvent playerDiedEvent)
        {
            Debug.Log("Player died");
        }
    }
}