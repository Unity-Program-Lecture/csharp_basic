namespace HuntMonster.Day07
{
    public class ItemBox : IIdentifier, IDamagable
    {
        private int _hp;
        private int _maxHp;

        /// <summary>
        /// 내구도
        /// </summary>
        public int Durability { get { return Hp; } }

        public string Name { get; private set; }

        public int Hp
        {
            get => _hp;

            protected set
            {
                _hp = value;

                if (_hp > _maxHp)
                {
                    _hp = _maxHp;
                }
                else if (_hp < 0)
                {
                    _hp = 0;
                }
            }
        }

        public int MaxHp
        {
            get
            {
                return _maxHp;
            }

            protected set
            {
                if (value > 0)
                {
                    _maxHp = value;
                }

                if (Hp > _maxHp)
                {
                    Hp = _maxHp;
                }

                Console.WriteLine("Warning!");
            }
        }

        public bool IsDead
        {
            get
            {
                return Hp <= 0;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">이름</param>
        /// <param name="maxDurability">최대 내구도</param>
        public ItemBox(string name, int maxDurability)
        {
            Name = name;
            _hp = _maxHp = maxDurability;
        }

        public void TakeDamage(int damage)
        {
            if (Hp <= damage)
            {
                Destoy();

                return;
            }

            Hp -= damage;

            Console.WriteLine($"<{Name}>가(이) [{damage}]만큼 내구도가 감소했습니다. 내구도 : [{Durability}]");
        }

        public string GetStatusText()
        {
            return $"내구도 : [{Hp}/{MaxHp}]";
        }

        private void Destoy()
        {
            Hp = 0;

            Console.WriteLine($"<{Name}>가(이) 파괴되었습니다.");
        }
    }
}
