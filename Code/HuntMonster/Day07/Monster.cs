using static HuntMonster.Day07.ICarryingItem;

namespace HuntMonster.Day07
{
    class Monster : Creature, ICarryingItem
    {
        public event OnDropItem OnDropItemEvent;

        public Item Item { get; private set; }

        public Monster(string name, int maxHp, int atk, int healAmount) : base(name, maxHp, atk, healAmount)
        {
            OnDeadEvent += () =>
            {
                if (Item is not null)
                {
                    OnDropItemEvent?.Invoke(Item);
                }
            };
        }

        public void SetDropItem(Item dropItem)
        {
            Item = dropItem;
        }

        public virtual void AIAction(Creature target)
        {
            if (Hp <= MaxHp / 3)
            {
                Heal();
            }
            else
            {
                Attack(target);
            }
        }
    }
}
