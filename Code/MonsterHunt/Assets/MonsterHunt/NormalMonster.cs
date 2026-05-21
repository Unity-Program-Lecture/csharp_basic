using UnityEngine;

namespace MonsterHunt
{
    public class NormalMonster : Monster
    {
        public override void OnDead()
        {
            GameSystem.Log($"{Name}이(가) 아이템을 떨어뜨렸습니다.");
        }
    }
}
