using UnityEngine;
using ProjectFukalite.Interfaces;
namespace ProjectFukalite.Data.Containment
{
    public class EnemyReferencer : MonoBehaviour
    {
        public MonoBehaviour enemyScript;

        public void Damage(int _amnt)
        {
            IEnemy enemy = enemyScript as IEnemy;
            enemy.Damage(_amnt);
        }
    }
}