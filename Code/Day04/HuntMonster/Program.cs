namespace Day04
{
    class Creature
    {
        public string Name;
        public int MaxHp;
        public int Hp;
        public int Atk;
        public int HealAmount;

        public Creature(string name, int maxHp, int atk, int healAmount)
        {
            Name = name;
            Hp = MaxHp = maxHp;
            Atk = atk;
            HealAmount = healAmount;
        }

        public bool IsDead()
        {
            return Hp <= 0;
        }

        public void Attack(Creature target)
        {
            Console.WriteLine($"<{Name}>가(이) <{target.Name}>을(를) [{Atk}]의 공격력으로 공격했습니다.");

            target.TakeDamage(Atk);
        }

        public virtual void Heal(int healAmount)
        {
            Hp += healAmount;

            if (Hp > MaxHp)
            {
                Hp = MaxHp;
            }

            Console.WriteLine($"<{Name}>가(이) [{healAmount}]만큼 회복했습니다. Hp : [{Hp}]");
        }

        public void Heal()
        {
            Heal(HealAmount);
        }

        protected virtual void TakeDamage(int damage)
        {
            if (Hp <= damage)
            {
                Die();

                return;
            }

            Hp -= damage;

            Console.WriteLine($"<{Name}>가(이) [{damage}]의 피해를 입었습니다. Hp : [{Hp}]");
        }

        private void Die()
        {
            Console.WriteLine($"<{Name}>가(이) 사망했습니다.");

            Hp = 0;
        }
    }

    class Player : Creature
    {
        public Player(string name, int maxHp, int atk, int healAmount) : base(name, maxHp, atk, healAmount)
        {
        }
    }

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

    class HuntMonster
    {
        static void Main(string[] args)
        {
            Console.Write("플레이어 이름을 입력하세요 : ");

            Player player = new Player(Console.ReadLine(), 100, 10, 40);

            Monster[] monsters = new Monster[]
            {
                new Monster("슬라임", 40, 10, 2),
                new Monster("오크", 70, 20, 4),
                new Skeleton("해골", 50, 10, 0)
            };

            Console.WriteLine();
            Console.WriteLine("몬스터들이 나타났습니다!");

            for (int i = 0; i < monsters.Length; ++i)
            {
                Monster monster = monsters[i];

                Console.WriteLine($"{i + 1}. {monster.Name} HP : [{monster.Hp}] / ATK : [{monster.Atk}] / HEAL : [{monster.HealAmount}]");
            }

            Console.WriteLine();

            int turnCount = 1;

            int aliveMonsterCount = monsters.Length;
            Monster[] aliveMonsters = new Monster[aliveMonsterCount];

            for (int i = 0; i < monsters.Length; ++i)
            {
                aliveMonsters[i] = monsters[i];
            }

            while (!player.IsDead() && aliveMonsterCount > 0)
            {
                Console.WriteLine($"현재 {player.Name}의 Hp : [{player.Hp}]");
                Console.WriteLine($"현재 턴[{turnCount}]에 할 행동을 선택하세요.\n1. 공격\n2. 회복");

                bool isValidInput = true;

                Console.Write(">> ");
                switch (Console.ReadLine())
                {
                    case "1":
                        {
                            Console.WriteLine("공격할 몬스터를 선택하세요.");

                            for (int i = 0; i < aliveMonsterCount; ++i)
                            {
                                Monster aliveMonster = aliveMonsters[i];

                                Console.WriteLine($"{i + 1}. {aliveMonster.Name} HP : [{aliveMonster.Hp}]");
                            }

                            Console.Write(">> ");
                            if (!int.TryParse(Console.ReadLine(), out int monsterIndex) || monsterIndex < 1 || monsterIndex > aliveMonsterCount)
                            {
                                isValidInput = false;
                            }
                            else
                            {
                                player.Attack(aliveMonsters[monsterIndex - 1]);
                            }

                            break;
                        }

                    case "2":
                        {
                            Console.WriteLine("회복 대상을 선택하세요.");

                            Console.WriteLine($"0. {player.Name} HP : [{player.Hp}]");

                            for (int i = 0; i < aliveMonsterCount; ++i)
                            {
                                Monster aliveMonster = aliveMonsters[i];

                                Console.WriteLine($"{i + 1}. {aliveMonster.Name} HP : [{aliveMonster.Hp}]");
                            }

                            Console.Write(">> ");
                            if (int.TryParse(Console.ReadLine(), out int healTargetNumber))
                            {
                                if (healTargetNumber == 0)
                                {
                                    player.Heal();
                                }
                                else if (healTargetNumber <= aliveMonsterCount)
                                {
                                    aliveMonsters[healTargetNumber - 1].Heal(player.HealAmount);
                                }
                                else
                                {
                                    isValidInput = false;
                                }
                            }
                        }
                        break;

                    default:
                        isValidInput = false;
                        break;
                }

                if (!isValidInput)
                {
                    Console.WriteLine("입력이 잘못되었습니다.");

                    continue;
                }

                aliveMonsterCount = 0;

                for (int i = 0; i < monsters.Length; ++i)
                {
                    Monster monster = monsters[i];
                    if (monster.IsDead())
                    {
                        continue;
                    }

                    monster.AIAction(player);

                    aliveMonsters[aliveMonsterCount] = monster;
                    aliveMonsterCount++;
                }

                turnCount++;

                Console.WriteLine();
            }

            if (player.IsDead())
            {
                Console.WriteLine("패배했습니다...");
            }
            else
            {
                Console.WriteLine("승리했습니다!!!");
            }
        }
    }
}
