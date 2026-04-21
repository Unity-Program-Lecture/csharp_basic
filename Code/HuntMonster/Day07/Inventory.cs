using System.Collections.Generic;

namespace HuntMonster.Day07
{
    public class Inventory
    {
        private Dictionary<string, Item> _bag;

        public IReadOnlyDictionary<string, Item> Bag => _bag;

        public Inventory()
        {
            _bag = new Dictionary<string, Item>();
        }

        public void AddItem(Item item)
        {
            if (item.Count <= 0)
            {
                return;
            }

            if (_bag.TryGetValue(item.Name, out Item curItem))
            {
                curItem.Count += item.Count;
            }
            else
            {
                _bag[item.Name] = item;
            }

            Console.WriteLine($"{item.Name} 아이템을 {item.Count}개 얻었습니다. 현재 {_bag[item.Name].Count}개 보유중입니다.");
        }

        public void RemoveItem(string itemName, int count)
        {
            if (_bag.TryGetValue(itemName, out Item curItem))
            {
                curItem.Count -= count;

                if (curItem.Count <= 0)
                {
                    _bag.Remove(itemName);
                }
            }
        }

        public void RemoveItem(string itemName)
        {
            RemoveItem(itemName, 1);
        }

        public bool TryGetItem(string name, out Item item)
        {
            return _bag.TryGetValue(name, out item);
        }

        public Item GetItem(string name)
        {
            // 아래 3가지 경우 중에 한가지를 보통 사용함.

            // 1. 가장 권장
            if (_bag.TryGetValue(name, out Item curItem))
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
            foreach (KeyValuePair<string, Item> pair in _bag)
            {
                pair.Value.Print();
            }
        }
    }
}
