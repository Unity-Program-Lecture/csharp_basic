using System.Collections.Generic;

namespace HuntMonster
{
    using Day07;

    public class Program
    {
        static void Swap<T>(ref T a, ref T b)
            where T : struct
        {
            T temp = a;
            a = b;
            b = temp;
        }

        static void Main(string[] args)
        {
            List<IDamagable> damagables = new List<IDamagable>(); // 현재 데미지를 입힐 수 있는 객체 리스트
            List<IRecovable> recovables = new List<IRecovable>(); // 현재 회복 가능한 객체 리스트            

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

                monster.OnDeadEvent += () =>
                {
                    monsters.Remove(monster);
                    damagables.Remove(monster);
                    recovables.Remove(monster);
                };
                monster.OnDropItemEvent += dropItem => inventory.AddItem(dropItem);

                damagables.Add(monster);
                recovables.Add(monster);
            }

            Console.WriteLine();

            #endregion

            #region spawn item box

            ItemBox itemBox = new ItemBox("[?] 상자", 4, new PotionItem("포션(초대형)", 10, 200));
            itemBox.OnDeadEvent += () =>
            {
                damagables.Remove(itemBox);
            };
            itemBox.OnDropItemEvent += dropItem => inventory.AddItem(dropItem);

            damagables.Add(itemBox);

            Console.WriteLine("아이템이 들어있을지도 모르는 상자가 나타났습니다!");
            Console.WriteLine(itemBox.GetStatusText());
            Console.WriteLine();

            #endregion

            int turnCount = 1;

            List<Item> currentItemsInBag = new List<Item>(); // 현재 가방에 있는 아이템 리스트

            while (!player.IsDead && monsters.Count > 0)
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

                            isValidInput = TrySelectTarget<IDamagable>(damagables, out int selectedIndex);
                            if (isValidInput)
                            {
                                player.Attack(damagables[selectedIndex]);
                            }

                            break;
                        }

                    case "2":
                        {
                            Console.WriteLine();
                            Console.WriteLine("회복 대상을 선택하세요.");

                            isValidInput = TrySelectTarget<IRecovable>(recovables, out int selectedIndex);
                            if (isValidInput)
                            {
                                recovables[selectedIndex].Heal(player.HealAmount);
                            }

                            break;
                        }

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

                            isValidInput = TrySelectTarget<Item>(currentItemsInBag, out int selectedIndex);
                            if (isValidInput)
                            {
                                Item itemWillUse = currentItemsInBag[selectedIndex];

                                inventory.TryUseItem(itemWillUse.Name, player);
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

                foreach (Monster monster in monsters)
                {
                    monster.AIAction(player);
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

        /// <summary>
        /// 리스트에서 대상을 선택한다.
        /// </summary>
        /// <typeparam name="T">선택할 대상의 타입</typeparam>
        /// <param name="targets">선택할 대상 리스트</param>
        /// <param name="selectedTargetIndex">선택된 대상의 인덱스</param>
        /// <returns>제대로 선택했다면 true, 아니면 false</returns>
        private static bool TrySelectTarget<T>(IReadOnlyList<T> targets, out int selectedTargetIndex)
            where T : IIdentifier
        {
            for (int i = 0; i < targets.Count; ++i)
            {
                T target = targets[i];

                Console.WriteLine($"{i + 1}. {target.GetStatusText()}");
            }

            Console.Write(">> ");
            if (int.TryParse(Console.ReadLine(), out int targetNumber) &&
                targetNumber >= 1 &&
                targetNumber <= targets.Count)
            {
                selectedTargetIndex = targetNumber - 1;

                return true;
            }

            Console.WriteLine();

            selectedTargetIndex = -1;

            return false;
        }
    }
}