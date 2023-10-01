using System.Collections.Generic;

namespace _Game.Scripts.Enemies
{
    public class EnemyRegistry
    {
        public readonly List<EnemyController> Enemies = new();
        
        public void AddEnemy(EnemyController enemy)
        {
            Enemies.Add(enemy);
        }
        
        public void RemoveEnemy(EnemyController enemy)
        {
            Enemies.Remove(enemy);
        }
    }
}