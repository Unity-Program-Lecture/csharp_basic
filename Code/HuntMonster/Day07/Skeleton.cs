namespace HuntMonster.Day07
{
    class Skeleton : Monster
    {
        public Skeleton(string name, int maxHp, int atk, int healAmount) : base(name, maxHp, atk, healAmount)
        {
        }

        public override void Heal(int healAmount)
        {
            TakeDamage(healAmount);
        }

        public override void AIAction(Creature target)
        {
            Attack(target);
        }
    }
}
