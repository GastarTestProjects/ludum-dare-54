using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts
{
    public class TempMusicManager : MonoBehaviour
    {
        public static TempMusicManager Instance { get; private set; }
        [SerializeField] private float fadeDuration;
        [SerializeField] private AudioSource menuMusicAudioSource;
        [SerializeField] private AudioSource gameMusicAudioSource;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            menuMusicAudioSource.loop = true;
            gameMusicAudioSource.loop = true;
        }

        public void PlayMenuMusic()
        {
            if (!menuMusicAudioSource.isPlaying)
                menuMusicAudioSource.Play();
            menuMusicAudioSource.volume = 0f;

            menuMusicAudioSource.DOFade(1f, fadeDuration);
            gameMusicAudioSource.DOFade(0f, fadeDuration).OnComplete(() => gameMusicAudioSource.Stop());
        }

        public void PlayGameMusic()
        {
            if (!gameMusicAudioSource.isPlaying)
                gameMusicAudioSource.Play();
            gameMusicAudioSource.volume = 0f;

            gameMusicAudioSource.DOFade(1f, fadeDuration);
            menuMusicAudioSource.DOFade(0f, fadeDuration).OnComplete(() => menuMusicAudioSource.Stop());
        }
    }
}