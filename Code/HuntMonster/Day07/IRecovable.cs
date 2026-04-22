namespace HuntMonster.Day07
{
    public interface IRecovable : IIdentifier, IMortal
    {
        int HealAmount { get; }

        void Heal(int healAmount);
        void Heal() => Heal(HealAmount);
    }
}
