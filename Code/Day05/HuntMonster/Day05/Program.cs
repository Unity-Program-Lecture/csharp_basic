using HuntMonster.Day05;

namespace Day05
{
    class Program
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

            ItemBox itemBox = new ItemBox("[?] 상자", 4);
            Console.WriteLine("아이템이 들어있을지도 모르는 상자가 나타났습니다!");

            Console.WriteLine($"{itemBox.Name} 내구도 : [{itemBox.Durability}]");

            int turnCount = 1;

            IDamagable[] damagables = new IDamagable[monsters.Length + 1];
            for (int i = 0; i < monsters.Length; ++i)
            {
                damagables[i] = monsters[i];
            }
            damagables[monsters.Length] = itemBox;

            IRecoverable[] recoverables = new IRecoverable[monsters.Length];
            for (int i = 0; i < monsters.Length; ++i)
            {
                recoverables[i] = monsters[i];
            }

            int aliveDamagableCount = damagables.Length;
            IDamagable[] aliveDamagables = new IDamagable[aliveDamagableCount];
            for (int i = 0; i < damagables.Length; ++i)
            {
                aliveDamagables[i] = damagables[i];
            }

            int aliveRecoverableCount = recoverables.Length;
            IRecoverable[] aliveRecoverables = new IRecoverable[aliveRecoverableCount];
            for (int i = 0; i < recoverables.Length; ++i)
            {
                aliveRecoverables[i] = recoverables[i];
            }

            while (!player.IsDead && aliveDamagableCount > 0)
            {
                Console.WriteLine($"현재 {player.Name}의 Hp : [{player.Hp}]");
                Console.WriteLine($"현재 턴[{turnCount}]에 할 행동을 선택하세요.\n1. 공격\n2. 회복");

                bool isValidInput = true;

                Console.Write(">> ");
                switch (Console.ReadLine())
                {
                    case "1":
                        {
                            Console.WriteLine("공격할 대상을 선택하세요.");

                            for (int i = 0; i < aliveDamagableCount; ++i)
                            {
                                IDamagable aliveDamagable = aliveDamagables[i];

                                Console.WriteLine($"{i + 1}. {aliveDamagable.Name} {aliveDamagable.GetStatusText()}");
                            }

                            Console.Write(">> ");
                            if (!int.TryParse(Console.ReadLine(), out int targetNumber) || targetNumber < 1 || targetNumber > aliveDamagableCount)
                            {
                                isValidInput = false;
                            }
                            else
                            {
                                player.Attack(aliveDamagables[targetNumber - 1]);
                            }

                            break;
                        }

                    case "2":
                        {
                            Console.WriteLine("회복 대상을 선택하세요.");

                            Console.WriteLine($"0. {player.Name} HP : [{player.Hp}]");

                            for (int i = 0; i < aliveRecoverableCount; ++i)
                            {
                                IRecoverable aliveRecoverable = aliveRecoverables[i];

                                Console.WriteLine($"{i + 1}. {aliveRecoverable.Name} {aliveRecoverable.GetStatusText()}");
                            }

                            Console.Write(">> ");
                            if (int.TryParse(Console.ReadLine(), out int healTargetNumber))
                            {
                                if (healTargetNumber == 0)
                                {
                                    player.Heal();
                                }
                                else if (healTargetNumber <= aliveRecoverableCount)
                                {
                                    aliveRecoverables[healTargetNumber - 1].Heal(player.HealAmount);
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

                aliveDamagableCount = 0;

                for (int i = 0; i < damagables.Length; ++i)
                {
                    IDamagable damagable = damagables[i];
                    if (damagable.IsDead)
                    {
                        continue;
                    }

                    Monster monster = damagable as Monster;
                    if (monster != null)
                    {
                        monster.AIAction(player);
                    }

                    aliveDamagables[aliveDamagableCount] = damagable;
                    aliveDamagableCount++;
                }

                aliveRecoverableCount = 0;

                for (int i = 0; i < recoverables.Length; ++i)
                {
                    IRecoverable recoverable = recoverables[i];
                    if (recoverable.IsDead)
                    {
                        continue;
                    }

                    aliveRecoverables[aliveRecoverableCount] = recoverable;
                    aliveRecoverableCount++;
                }

                turnCount++;

                Console.WriteLine();
            }

            if (player.IsDead)
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
