using System.Collections.Generic;

namespace MonsterHunt
{
    public class Storage<T> where T : class
    {
        private List<T> _items = new();

        public int Count => _items.Count;

        public void Save(T item)
        {
            _items.Add(item);
        }

        public T Get()
        {
            if (_items.Count == 0)
            {
                return default;
            }

            T item = _items[0];
            _items.RemoveAt(0);

            return item;
        }
    }
}