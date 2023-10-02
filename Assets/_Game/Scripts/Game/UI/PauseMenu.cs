using _Game.Scripts.Game.Events;
using TMPro;
using UnityEngine;
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
        private ScoreHandler _scoreHandler;

        [Inject]
        private void Construct(SignalBus signalBus, GameController gameController, ScoreHandler scoreHandler)
        {
            _scoreHandler = scoreHandler;
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
            continueButton.gameObject.SetActive(!playerDied);
            pauseTxt.gameObject.SetActive(!playerDied);
            playerDiedTxt.gameObject.SetActive(playerDied);
            playerDiedInfoContainer.gameObject.SetActive(playerDied);
            playerRecordTxt.text = $"Your score: {_scoreHandler.CurrentScore}";
            var recordScore = _scoreHandler.RecordScore;
            if (recordScore > 0)
            {
                playerRecordTxt.text += $"  Record score: {recordScore}";
            }
        }

        private void SubscribeButtons()
        {
            continueButton.onClick.AddListener(_gameController.Unpause);
            mainMenuButton.onClick.AddListener(_gameController.OpenMainMenu);
            restartButton.onClick.AddListener(_gameController.RestartGame);
        }
    }
}