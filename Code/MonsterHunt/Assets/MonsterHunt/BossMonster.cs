using UnityEngine;

namespace MonsterHunt
{
    public class BossMonster : Monster
    {
        [SerializeField] private int shield;

        public override void TakeDamage(int damage)
        {
            if (shield > 0)
            {
                shield -= damage;

                if (shield < 0)
                {
                    shield = 0;
                }
            }

            if (shield == 0)
            {
                base.TakeDamage(damage);
            }
        }

        public override void OnDead()
        {
            GameSystem.Log($"화려한 이펙트와 함께 {Name} 처치!");
        }
    }
}