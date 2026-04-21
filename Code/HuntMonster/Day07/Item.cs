using System.Collections.Generic;

namespace HuntMonster.Day07
{
    public class Item : IIdentifier
    {
        public string Name { get; set; }
        public int Count { get; set; }

        public Item(string name, int count)
        {
            Name = name;
            Count = count;
        }

        public Item(string name) : this(name, 1)
        {
        }

        public void UseTo(int count, Creature target)
        {
            if (Count <= 0)
            {
                Console.WriteLine($"남은 {Name} 아이템이 없습니다.");
                return;
            }

            int usableCount = Math.Min(count, Count);

            Console.WriteLine($"{Name} 아이템을 {usableCount}개 사용했습니다.");

            for (int i = 0; i < usableCount; i++)
            {
                UseEffect(target);
            }

            Count -= usableCount;
        }

        public void Print()
        {
            Console.WriteLine(GetStatusText());
        }

        public string GetStatusText()
        {
            return $"{Name} : {Count}개";
        }

        protected virtual void UseEffect(Creature target)
        {

        }
    }
}
