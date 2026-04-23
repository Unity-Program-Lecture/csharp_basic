namespace HuntMonster.Day07
{
    public interface IMortal : IIdentifier
    {
        delegate void OnDead();

        event OnDead OnDeadEvent;

        bool IsDead { get; }
    }
}
