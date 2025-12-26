using STH.Core;
using STH.Combat.Projectiles;
using UnityEngine;

namespace STH.Combat.Modifiers
{
    /// <summary>
    /// 관통 모디파이어 - 총알이 적을 관통
    /// </summary>
    public class PierceModifier : IBulletModifier
    {
        private readonly int pierceCount;
        private readonly float attackMultiplier = -0.3f;

        /// <summary>
        /// 관통 모디파이어를 생성합니다.
        /// </summary>
        /// <param name="pierceCount">관통 횟수</param>
        public PierceModifier(int pierceCount, float attackMultiplier = -0.3f)
        {
            this.pierceCount = pierceCount;
            this.attackMultiplier = attackMultiplier;
        }

        /// <summary>
        /// 총알 초기화 시 관통 카운트를 설정합니다.
        /// </summary>
        public void OnInitialize(Bullet bullet)
        {
            bullet.PierceCount += pierceCount;

        }

        public void OnHit(Bullet bullet, IDamageable target, LayerMask targetLayer)
        {
            bullet.SetDamageAndCritical(bullet.damage + bullet.damage * attackMultiplier);
        }
    }
}
