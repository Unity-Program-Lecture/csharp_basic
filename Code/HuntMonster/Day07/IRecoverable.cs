namespace HuntMonster.Day07
{
    public interface IRecoverable : IIdentifier, IMortal
    {
        int HealAmount { get; }

        void Heal(int healAmount);
        void Heal() => Heal(HealAmount);
    }
}
