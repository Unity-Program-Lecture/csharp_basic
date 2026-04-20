namespace HuntMonster.Day05
{
    public interface IIdentifier
    {
        string Name { get; }

        string GetStatusText();
    }
}
