namespace HuntMonster.Day05
{
    public interface IAttackable
    {
        int Atk { get; }

        void Attack(IDamagable target);
    }
}
