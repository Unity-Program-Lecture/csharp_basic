namespace HuntMonster.Day05
{
    public interface IDamagable : IIdentifier, IMortal
    {
        int Hp { get; }
        int MaxHp { get; }

        void TakeDamage(int damage);
    }
}
