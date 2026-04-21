namespace HuntMonster.Day07
{
    public interface IAttackable
    {
        int Atk { get; }

        void Attack(IDamagable target);
    }
}