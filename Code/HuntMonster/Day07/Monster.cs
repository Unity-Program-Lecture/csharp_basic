namespace HuntMonster.Day07
{
    class Monster : Creature
    {
        public event OnDropItem OnDropItemEvent;

        public Item DropItem { get; private set; }

        public Monster(string name, int maxHp, int atk, int healAmount) : base(name, maxHp, atk, healAmount)
        {
        }

        public void SetDropItem(Item dropItem)
        {
            DropItem = dropItem;
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

        protected override void Die()
        {
            base.Die();

            if (DropItem is not null)
            {
                OnDropItemEvent?.Invoke(DropItem);
            }
        }
    }
}
