using System;

namespace STH.Core
{
    /// <summary>
    /// 게임 스탯 데이터 클래스
    /// </summary>
    [Serializable]
    public class GameStats
    {
        public float damage = 10f;
        public float attackSpeed = 1f;
        public float hp = 100f;
        public float moveSpeed = 5f;

        public GameStats() { }

        public GameStats(float damage, float attackSpeed, float hp, float moveSpeed = 5f)
        {
            this.damage = damage;
            this.attackSpeed = attackSpeed;
            this.hp = hp;
            this.moveSpeed = moveSpeed;
        }

        public GameStats Clone()
        {
            return new GameStats(damage, attackSpeed, hp, moveSpeed);
        }
    }
}
