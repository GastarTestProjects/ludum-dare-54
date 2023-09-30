using _Game.Scripts.Game;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Game.Scripts.Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button startGame;
        [SerializeField] private Button exitGame;

        private void Start()
        {
            startGame.onClick.AddListener(LoadGameScene);

#if UNITY_WEBGL
exitGame.gameObject.SetActive(false);
#else
            exitGame.onClick.AddListener(ExitGame);
#endif
        }
        
        private void LoadGameScene()
        {
            SceneLoader.Instance.LoadGameScene();
        }

        private void ExitGame()
        {
            Application.Quit();
        }
    }
}