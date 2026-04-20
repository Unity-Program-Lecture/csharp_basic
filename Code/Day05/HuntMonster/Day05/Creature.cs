using HuntMonster.Day05;

namespace Day05
{
    class Creature : IIdentifier, IDamagable, IAttackable, IRecoverable, IMortal
    {
        #region IIdentifier 구현부

        public string Name { get; private set; }

        public string GetStatusText()
        {
            return $"Hp : [{Hp}/{MaxHp}], 공격력 : [{Atk}], 회복력 : [{HealAmount}]";
        }

        #endregion

        #region IDamageable 구현부

        private int _hp;
        private int _maxHp;

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

        public virtual void TakeDamage(int damage)
        {
            if (Hp <= damage)
            {
                Die();

                return;
            }

            Hp -= damage;

            Console.WriteLine($"<{Name}>가(이) [{damage}]의 피해를 입었습니다. Hp : [{Hp}]");
        }

        #endregion

        #region IAttackable 구현부

        public int Atk { get; private set; }

        public void Attack(IDamagable target)
        {
            Console.WriteLine($"<{Name}>가(이) [{Atk}]의 공격력으로 공격했습니다.");

            target.TakeDamage(Atk);
        }

        #endregion

        #region IRecoverable 구현부

        public int HealAmount { get; private set; }


        public virtual void Heal(int healAmount)
        {
            int oldHp = Hp;

            Hp += healAmount;

            int healed = Hp - oldHp;

            if (healed > 0)
            {
                Console.WriteLine($"<{Name}>가(이) [{Hp - oldHp}]만큼 회복했습니다. Hp : [{Hp}]");
            }
            else
            {
                Console.WriteLine($"<{Name}>는(은) 이미 최대 Hp입니다. Hp : [{Hp}]");
            }
        }

        public void Heal()
        {
            Heal(HealAmount);
        }

        #endregion

        #region IMortal 구현부

        public bool IsDead
        {
            get
            {
                return Hp <= 0;
            }
        }

        #endregion


        public Creature(string name, int maxHp, int atk, int healAmount)
        {
            Name = name;
            Hp = MaxHp = maxHp;
            Atk = atk;
            HealAmount = healAmount;
        }

        private void Die()
        {
            Console.WriteLine($"<{Name}>가(이) 사망했습니다.");

            Hp = 0;
        }
    }
}
