namespace HuntMonster.Day07
{
    public interface IDamagable : IIdentifier, IMortal
    {
        int Hp { get; }
        int MaxHp { get; }

        void TakeDamage(int damage);
    }
}
