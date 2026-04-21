namespace HuntMonster.Day07
{
    public delegate void OnDropItem(Item dropItem);

    public interface ICarryingItem
    {
        Item DropItem { get; }

        event OnDropItem OnDropItemEvent;
    }
}
