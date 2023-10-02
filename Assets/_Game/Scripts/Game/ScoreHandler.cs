using _Game.Scripts.Enemies;
using _Game.Scripts.Player;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Game
{
    public class ScoreHandler
    {
        private int currentScore;
        public int CurrentScore => currentScore;
        
        public int RecordScore => PlayerPrefs.GetInt("RecordScore", 0);
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            signalBus.Subscribe<EnemyDiedEvent>(OnEnemyKilled);
            signalBus.Subscribe<PlayerDiedEvent>(OnPlayerDied);
        }
        
        public void OnEnemyKilled(EnemyDiedEvent enemyDiedEvent)
        {
            if (enemyDiedEvent.Killed)
            {
                currentScore += 1;
            }
        }
        
        public void OnPlayerDied(PlayerDiedEvent playerDiedEvent)
        {
            if (currentScore > RecordScore)
            {
                PlayerPrefs.SetInt("RecordScore", currentScore);
            }
        }
    }
}