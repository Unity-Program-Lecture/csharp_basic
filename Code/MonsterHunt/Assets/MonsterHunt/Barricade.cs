using UnityEngine;

namespace MonsterHunt
{
    public class Barricade : MonoBehaviour, IDamageable
    {
        [SerializeField] private int hp;

        public bool IsDead => hp <= 0;

        public void TakeDamage(int damage)
        {
            hp -= damage;

            if (hp < 0)
            {
                GameSystem.Log($"[{name}] 장애물이 파손되었습니다!");
            }
        }
    }
}

