using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game.Scripts.Game
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance;

        [SerializeField] private CanvasGroup loadingPanel;
        [SerializeField] private float fadeTime = 0.2f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                DestroyImmediate(this);
                return;
            }

            loadingPanel.gameObject.SetActive(false);
            TempMusicManager.Instance.PlayMenuMusic();
        }

        public void LoadMenuScene()
        {
            loadingPanel.gameObject.SetActive(true);
            loadingPanel.alpha = 0;
            loadingPanel.DOFade(1, fadeTime)
                .OnComplete(
                    () =>
                    {
                        StartLoading(
                            "_Game/Scenes/MainMenuScene",
                            "_Game/Scenes/GameScene",
                            () => { TempMusicManager.Instance.PlayMenuMusic(); });
                    });
        }

        public void LoadGameScene()
        {
            loadingPanel.gameObject.SetActive(true);
            loadingPanel.alpha = 0;
            loadingPanel.DOFade(1, fadeTime)
                .OnComplete(
                    () =>
                    {
                        StartLoading(
                            "_Game/Scenes/GameScene",
                            "_Game/Scenes/MainMenuScene",
                            () => { TempMusicManager.Instance.PlayGameMusic(); });
                    });
        }

        private void StartLoading(string sceneName, string unloadSceneName, Action onComplete)
        {
            var operation = SceneManager.LoadSceneAsync(sceneName);
            operation.completed += _ =>
            {
                // loaderPanel.transform.DOKill();
                loadingPanel.DOFade(0, fadeTime)
                    .OnComplete(
                        () =>
                        {
                            loadingPanel.gameObject.SetActive(false);
                            SceneManager.UnloadSceneAsync(unloadSceneName);
                        });
                onComplete?.Invoke();
            };
        }
    }
}