using System.Collections.Generic;
using UnityEngine;

namespace MonsterHunt
{
    public class GameSystem : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private List<Monster> monsters;
        [SerializeField] private Barricade[] barricades;

        private static Stack<string> _logs = new();

        public static void Log(string msg)
        {
            _logs.Push(msg);
        }

        private IDamageable[] targets;
        private Storage<Monster> _monsterStorage;
        private Dictionary<string, Monster> _dicMonsters;
        private Queue<Monster> _spawnQueue;

        private int playerDamage = 200;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _monsterStorage = new Storage<Monster>();
            _dicMonsters = new();
            _spawnQueue = new();

            targets = new IDamageable[monsters.Count + barricades.Length];

            for (int i = 0; i < monsters.Count; i++)
            {
                targets[i] = monsters[i];

                _monsterStorage.Save(monsters[i]);

                _dicMonsters[monsters[i].Name] = monsters[i];
            }

            while (_monsterStorage.Count > 0)
            {
                Monster m = _monsterStorage.Get();

                GameSystem.Log($"저장된 몬스터<{m.Name}>을 꺼냈습니다.");
            }


            int barricadeIndex = 0;
            for (int i = monsters.Count; i < targets.Length; i++, barricadeIndex++)
            {
                targets[i] = barricades[barricadeIndex];
            }

            for (int i = 0; i < targets.Length; i++)
            {
                targets[i].TakeDamage(playerDamage);

                if (targets[i] is Monster monster && monster.IsDead)
                {
                    int exp = 100;
                    int level = player.Level;

                    LevelUp(ref exp, ref level);

                    player.Level = level;

                    if (TryGetLoot(monster, out string lootName))
                    {
                        GameSystem.Log($"{lootName}을(를) 획득했습니다.");
                    }
                }
            }

            for (int i = 0; i < 3; ++i)
            {
                Debug.Log(_logs.Pop());
            }
        }

        public void LevelUp(ref int exp, ref int level)
        {
            while (exp >= 100)
            {
                level++;
                exp -= 100;
            }
        }

        public bool TryGetLoot(Monster m, out string lootName)
        {
            if (m.IsDead)
            {
                lootName = m.Reward.itemName;

                return true;
            }

            lootName = "없음";

            return false;
        }

        public void Spawn<T>(T entity) where T : IDamageable
        {

        }
    }
}