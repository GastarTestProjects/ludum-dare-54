using _Game.Scripts.Game.Events;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Game.UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject playerDiedInfo;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            signalBus.Subscribe<PauseEvent>(OnPauseGame);
        }
        
        
        private void OnPauseGame(PauseEvent pauseEvent)
        {
            pauseMenu.SetActive(pauseEvent.Paused);
            playerDiedInfo.SetActive(pauseEvent.PlayerDied);
        }
    }
}