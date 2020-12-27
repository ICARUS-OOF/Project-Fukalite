using ProjectFukalite.Handlers;
using ProjectFukalite.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectFukalite.Entities
{
    public class Dummy : MonoBehaviour, IEnemy
    {
        [SerializeField] private float maxHealth = 50;

        [SerializeField] private Slider healthBarSlider;
        
        [SerializeField] private GameObject rockParticles;
        [SerializeField] private Transform particlePoint;

        public float Health { get; set; }
        public bool isDead { get; set; }

        private void Start()
        {
            Health = maxHealth;
        }

        private void Update()
        {
            healthBarSlider.value = Health / maxHealth;
        }

        public void Damage(float _damage)
        {
            if (isDead)
            { return; }
            Health -= _damage;
            if (Health <= 0)
            {
                isDead = true;
                Die();
            }
        }

        public void Die()
        {
            AudioHandler.PlaySoundEffect("Debris");
            GameObject _particles = Instantiate(rockParticles, particlePoint.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}