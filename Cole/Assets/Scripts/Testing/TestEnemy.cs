using ProjectFukalite.Interfaces;
using System;
using UnityEngine;
namespace ProjectFukalite.Testing
{
    public class TestEnemy : MonoBehaviour, IEnemy
    {
        [SerializeField] private float _Health;
        public float Health { get { return _Health; } set { _Health = value; } }
        [SerializeField] private bool _isDead;
        public bool isDead { get { return _isDead; } set { _isDead = value; } }

        public void Damage(float _damage)
        {
            Health -= _damage;
            Debug.Log(transform.name + " has taken " + _damage + " damage.");
            if (Health <= 0f)
            {
                Die();
            }
        }

        public void Die()
        {
            Debug.Log(transform.name + " has died!");
        }
    }
}