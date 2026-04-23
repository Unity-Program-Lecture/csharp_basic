namespace HuntMonster.Day07
{
    public interface ICarryingItem
    {
        delegate void OnDropItem(Item dropItem);

        event OnDropItem OnDropItemEvent;

        Item Item { get; }
    }
}
