namespace HuntMonster.Day07
{
    public class PotionItem : Item
    {
        private int _healAmount;

        public PotionItem(string name, int count, int healAmount) : base(name, count)
        {
            _healAmount = healAmount;
        }

        protected override void UseEffect(Creature target)
        {
            target.Heal(_healAmount);
        }
    }
}