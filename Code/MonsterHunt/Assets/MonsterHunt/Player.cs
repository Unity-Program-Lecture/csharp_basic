using System.Drawing;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

namespace MonsterHunt
{
    public class Player : MonoBehaviour, IDamageable
    {
        [SerializeField] private int hp;
        [SerializeField] private int exp;
        [SerializeField] private int level;

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

        public int Exp
        {
            get => exp;
            set => exp = value;
        }

        public int Level
        {
            get => level;
            set => level = value;
        }

        public bool IsDead => HP <= 0;

        public virtual void TakeDamage(int damage)
        {
            HP -= damage;

            GameSystem.Log($"{name}이(가) {damage}의 피해를 입었습니다.");

            if (hp <= 0)
            {
                OnDead();
            }
        }

        public virtual void OnDead()
        {
            GameSystem.Log($"{name}이(가) 사라졌습니다.");
        }
    }
}

