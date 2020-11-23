namespace ProjectFukalite.Interfaces
{
    public interface IEnemy
    {
        float Health { get; set; }
        bool isDead { get; set; }
        void Damage(float _damage);
        void Die();
    }
}