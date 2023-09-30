using _Game.Scripts.Game.Events;
using _Game.Scripts.Game.Models;
using _Game.Scripts.Game.Player;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Game
{
    public class GameController : MonoBehaviour
    {
        private OtherInput _otherInput;
        private SignalBus _signalBus;

        [Inject]
        private void Construct(OtherInput otherInput, SignalBus signalBus)
        {
            _signalBus = signalBus;
            _otherInput = otherInput;
            signalBus.Subscribe<PlayerDiedEvent>(OnPlayerDied);
        }

        private void Update()
        {
            if (_otherInput.pausePressed)
            {
                if (Time.timeScale == 0)
                    Unpause();
                else
                    Pause();
            }
        }

        public void Pause()
        {
            Debug.Log("Pause pressed");
            Time.timeScale = 0;
            _signalBus.Fire(new PauseEvent(true, false));
        }
        
        public void Unpause()
        {
            Debug.Log("Unpause pressed");
            Time.timeScale = 1;
            _signalBus.Fire(new PauseEvent(false, false));
        }

        public void OpenMainMenu()
        {
            Time.timeScale = 1f;
            SceneLoader.Instance.LoadMenuScene();
        }

        private void OnPlayerDied(PlayerDiedEvent playerDiedEvent)
        {
            Debug.Log("Player died");
            Time.timeScale = 0;
            _signalBus.Fire(new PauseEvent(true, true));
        }
    }
}