using System.Collections.Generic;

namespace HuntMonster
{
    using Day07;

    public class Program
    {
        static void Main(string[] args)
        {
            #region spawn player

            string? playerName;

            do
            {
                Console.Write("플레이어 이름을 입력하세요 : ");

                playerName = Console.ReadLine();
                if (!string.IsNullOrEmpty(playerName))
                {
                    break;
                }

                Console.WriteLine("입력이 잘못되었습니다.");
            }
            while (true);

            Player player = new Player(playerName, 100, 10, 40);

            Console.WriteLine();

            #endregion

            #region create inventory

            Inventory inventory = new Inventory();
            inventory.AddItem(new PotionItem("포션(소)", 2, 30));
            inventory.AddItem(new PotionItem("포션(대)", 2, 100));

            Console.WriteLine("현재 가방");
            inventory.PrintItems();

            Console.WriteLine();

            #endregion

            #region spawn monsters

            List<Monster> monsters = new List<Monster>
            {
                new Monster("슬라임", 40, 10, 2),
                new Monster("오크", 70, 20, 4),
                new Skeleton("해골", 50, 10, 0)
            };

            Console.WriteLine("몬스터들이 나타났습니다!");

            for (int i = 0; i < monsters.Count; ++i)
            {
                Monster monster = monsters[i];

                Console.WriteLine($"{i + 1}. {monster.GetStatusText()}");

                monster.SetDropItem(new PotionItem("포션(대)", 1, 100));

                monster.OnDropItemEvent += dropItem => inventory.AddItem(dropItem);
            }

            Console.WriteLine();

            #endregion

            #region spawn item box

            ItemBox itemBox = new ItemBox("[?] 상자", 4, new PotionItem("포션(초대형)", 10, 200));
            itemBox.OnDropItemEvent += dropItem => inventory.AddItem(dropItem);

            Console.WriteLine("아이템이 들어있을지도 모르는 상자가 나타났습니다!");
            Console.WriteLine(itemBox.GetStatusText());
            Console.WriteLine();

            #endregion

            int turnCount = 1;

            List<IDamagable> damagables = new List<IDamagable>(monsters);
            damagables.Add(itemBox);

            List<IRecovable> recovables = new List<IRecovable>(monsters);
            // 회복가능 리스트에 플레이어를 가장 앞에 추가
            recovables.Insert(0, player);

            List<IDamagable> aliveDamagables = new List<IDamagable>(damagables);
            List<IRecovable> aliveRecovables = new List<IRecovable>(recovables);

            List<Item> currentItemsInBag = new List<Item>();

            while (!player.IsDead && aliveDamagables.Count > 0)
            {
                Console.WriteLine(player.GetStatusText());

                // 가방에 아이템이 하나도 없으면 아이템 사용 선택지를 보여주지 않는다.
                if (inventory.Bag.Count == 0)
                {
                    Console.WriteLine($"현재 턴[{turnCount}]에 할 행동을 선택하세요.\n1. 공격\n2. 회복");
                }
                else
                {
                    Console.WriteLine($"현재 턴[{turnCount}]에 할 행동을 선택하세요.\n1. 공격\n2. 회복\n3. 아이템 사용");
                }

                bool isValidInput = true;

                Console.Write(">> ");
                switch (Console.ReadLine())
                {
                    case "1":
                        {
                            Console.WriteLine();
                            Console.WriteLine("공격할 대상을 선택하세요.");

                            if (TrySelectTarget<IDamagable>(aliveDamagables, out int targetNumber))
                            {
                                player.Attack(aliveDamagables[targetNumber - 1]);
                            }
                            else
                            {
                                isValidInput = false;
                            }

                            break;
                        }

                    case "2":
                        {
                            Console.WriteLine();
                            Console.WriteLine("회복 대상을 선택하세요.");

                            if (TrySelectTarget<IRecovable>(aliveRecovables, out int targetNumber))
                            {
                                aliveRecovables[targetNumber - 1].Heal(player.HealAmount);
                            }
                            else
                            {
                                isValidInput = false;
                            }
                        }
                        break;

                    case "3":
                        {
                            // 가방이 비어있는 상태로 아이템 사용하려고 하면 잘못된 입력으로 처리한다.
                            if (inventory.Bag.Count == 0)
                            {
                                isValidInput = false;

                                break;
                            }

                            Console.WriteLine();
                            Console.WriteLine("사용할 아이템을 선택하세요.");

                            currentItemsInBag.Clear();
                            currentItemsInBag.AddRange(inventory.Bag.Values);

                            if (TrySelectTarget<Item>(currentItemsInBag, out int targetNumber))
                            {
                                Item itemWillUse = currentItemsInBag[targetNumber - 1];

                                inventory.TryUseItem(itemWillUse.Name, player);
                            }
                            else
                            {
                                isValidInput = false;
                            }

                            break;
                        }

                    default:
                        isValidInput = false;
                        break;
                }

                if (!isValidInput)
                {
                    Console.WriteLine("입력이 잘못되었습니다.");

                    continue;
                }

                aliveDamagables.Clear();

                for (int i = 0; i < damagables.Count; ++i)
                {
                    IDamagable damagable = damagables[i];
                    if (damagable.IsDead)
                    {
                        continue;
                    }

                    if (damagable is Monster monster)
                    {
                        monster.AIAction(player);
                    }

                    aliveDamagables.Add(damagable);
                }

                aliveRecovables.Clear();

                for (int i = 0; i < recovables.Count; ++i)
                {
                    IRecovable recoverable = recovables[i];
                    if (recoverable.IsDead)
                    {
                        continue;
                    }

                    aliveRecovables.Add(recoverable);
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

        static bool TrySelectTarget<T>(List<T> targets, out int targetNumber)
            where T : IIdentifier
        {
            for (int i = 0; i < targets.Count; ++i)
            {
                T target = targets[i];

                Console.WriteLine($"{i + 1}. {target.GetStatusText()}");
            }

            Console.Write(">> ");
            if (!int.TryParse(Console.ReadLine(), out targetNumber) ||
                targetNumber < 1 ||
                targetNumber > targets.Count)
            {
                return false;
            }

            Console.WriteLine();

            return true;
        }
    }
}