using System.Collections.Generic;

namespace HuntMonster.Day07
{
    public class Inventory
    {
        public Dictionary<string, Item> Bag;

        public Inventory()
        {
            Bag = new Dictionary<string, Item>();
        }

        public void AddItem(Item item)
        {
            if (item.Count <= 0)
            {
                return;
            }

            if (Bag.TryGetValue(item.Name, out Item curItem))
            {
                curItem.Count += item.Count;
            }
            else
            {
                Bag[item.Name] = item;
            }
        }

        public void RemoveItem(string itemName, int count)
        {
            if (Bag.TryGetValue(itemName, out Item curItem))
            {
                curItem.Count -= count;

                if (curItem.Count <= 0)
                {
                    Bag.Remove(itemName);
                }
            }
        }

        public void RemoveItem(string itemName)
        {
            RemoveItem(itemName, 1);
        }

        public bool TryGetItem(string name, out Item item)
        {
            return Bag.TryGetValue(name, out item);
        }

        public Item GetItem(string name)
        {
            // 아래 3가지 경우 중에 한가지를 보통 사용함.

            // 1. 가장 권장
            if (Bag.TryGetValue(name, out Item curItem))
            {
                return curItem;
            }

            // 2. 일반적인 경우
            //if (Bag.ContainsKey(name))
            //{
            //    return Bag[name];
            //}

            // 3. 예외처리
            //try
            //{
            //    return Bag[name];
            //}
            //catch(Exception ex)
            //{
            //    // 예외 출력
            //    Console.WriteLine(ex.ToString());
            //}

            // 찾지 못한 경우
            return null;
        }

        public bool TryUseItem(string itemName, int count, Creature target)
        {
            if (TryGetItem(itemName, out Item item))
            {
                item.UseTo(count, target);

                if (item.Count <= 0)
                {
                    RemoveItem(itemName);
                }

                return true;
            }

            return false;
        }

        public bool TryUseItem(string itemName, Creature target)
        {
            return TryUseItem(itemName, 1, target);
        }

        public void PrintItems()
        {
            foreach (KeyValuePair<string, Item> pair in Bag)
            {
                pair.Value.Print();
            }
        }
    }
}
