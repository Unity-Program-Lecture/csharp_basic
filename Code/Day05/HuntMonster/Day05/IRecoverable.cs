namespace HuntMonster.Day05
{
    public interface IRecoverable : IIdentifier, IMortal
    {
        int HealAmount { get; }

        void Heal(int healAmount);
        void Heal() => Heal(HealAmount);
    }
}
