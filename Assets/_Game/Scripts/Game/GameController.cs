using _Game.Scripts.Game.Events;
using _Game.Scripts.Game.Models;
using _Game.Scripts.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace _Game.Scripts.Game
{
    public class GameController : MonoBehaviour
    {
        private OtherInput _otherInput;
        private SignalBus _signalBus;
        
        private bool _playerDied;

        [Inject]
        private void Construct(OtherInput otherInput, SignalBus signalBus)
        {
            _playerDied = false;
            _signalBus = signalBus;
            _otherInput = otherInput;
            signalBus.Subscribe<PlayerDiedEvent>(OnPlayerDied);
        }

        private void Update()
        {
            if (_otherInput.pausePressed && !_playerDied)
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
            _playerDied = false;
        }

        public void OpenMainMenu()
        {
            Time.timeScale = 1f;
            SceneLoader.Instance.LoadMenuScene();
        }

        public void RestartGame()
        {
            _playerDied = false;
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnPlayerDied(PlayerDiedEvent playerDiedEvent)
        {
            Debug.Log("Player died");
            _playerDied = true;
            Time.timeScale = 0;
            _signalBus.Fire(new PauseEvent(true, true));
        }
    }
}