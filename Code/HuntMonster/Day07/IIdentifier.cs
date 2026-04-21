namespace HuntMonster.Day07
{
    public interface IIdentifier
    {
        string Name { get; }

        string GetStatusText();
    }
}