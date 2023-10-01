using _Game.Scripts.Game.Events;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace _Game.Scripts.Game.UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenu;
        
        [Header("Text")]
        [SerializeField] private TMP_Text pauseTxt;
        [SerializeField] private TMP_Text playerDiedTxt;
        [SerializeField] private RectTransform playerDiedInfoContainer;
        [SerializeField] private TMP_Text playerRecordTxt;
        
        [Header("Buttons")]
        [SerializeField] private Button continueButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button mainMenuButton;
        
        private GameController _gameController;

        [Inject]
        private void Construct(SignalBus signalBus, GameController gameController)
        {
            pauseMenu.SetActive(false);
            
            _gameController = gameController;
            signalBus.Subscribe<PauseEvent>(OnPauseGame);
            
            SubscribeButtons();
        }
        
        
        private void OnPauseGame(PauseEvent pauseEvent)
        {
            pauseMenu.SetActive(pauseEvent.Paused);
            if (pauseEvent.Paused)
                ShowElements(pauseEvent.PlayerDied, pauseEvent.PlayerTime);
        }

        private void ShowElements(bool playerDied, float playerTime)
        {
            pauseTxt.gameObject.SetActive(!playerDied);
            playerDiedTxt.gameObject.SetActive(playerDied);
            playerDiedInfoContainer.gameObject.SetActive(playerDied);
            playerRecordTxt.text = $"Your time: {playerTime}";
        }

        private void SubscribeButtons()
        {
            continueButton.onClick.AddListener(_gameController.Unpause);
            mainMenuButton.onClick.AddListener(_gameController.OpenMainMenu);
            restartButton.onClick.AddListener(_gameController.RestartGame);
        }
    }
}