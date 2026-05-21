namespace MonsterHunt
{
    public interface IDamageable
    {
        bool IsDead { get; }

        void TakeDamage(int damage);
    }
}
