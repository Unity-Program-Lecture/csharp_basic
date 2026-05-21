using UnityEngine;

namespace MonsterHunt
{
    public class Monster : MonoBehaviour, IDamageable
    {
        [SerializeField] private string monsterName;
        [SerializeField] private int hp;
        [SerializeField] private Reward reward;
        [SerializeField] private Point point;

        public string Name => monsterName;

        public int HP
        {
            get => hp;

            set
            {
                if (value < 0)
                {
                    hp = 0;
                }
                else
                {
                    hp = value;
                }

            }
        }

        public bool IsDead => HP <= 0;

        public Reward Reward => reward;        

        public virtual void TakeDamage(int damage)
        {
            HP -= damage;

            GameSystem.Log($"{monsterName}이(가) {damage}의 피해를 입었습니다.");

            if (hp <= 0)
            {
                OnDead();
            }
        }

        public virtual void OnDead()
        {
            GameSystem.Log($"{monsterName}이(가) 사라졌습니다.");
            GameSystem.Log($"({point.x}, {point.y}) 위치에서 '{monsterName}' 처치! [보상: {reward.itemName}, 동전 {reward.gold}개]");
        }
    }
}

