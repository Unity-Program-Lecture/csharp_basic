namespace HuntMonster.Day07
{
    class Monster : Creature
    {
        public Monster(string name, int maxHp, int atk, int healAmount) : base(name, maxHp, atk, healAmount)
        {
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
