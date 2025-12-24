using UnityEngine;
using STH.Core;
using STH.Combat.Projectiles;
using STH.Systems;

namespace STH.Combat.Modifiers
{
    /// <summary>
    /// 독 모디파이어 - 적에게 지속 데미지(DoT)를 적용
    /// </summary>
    public class PoisonModifier : IBulletModifier
    {
        private readonly float damagePerTick;
        private readonly float duration;
        private readonly float tickInterval;

        /// <summary>
        /// 독 모디파이어를 생성합니다.
        /// </summary>
        /// <param name="damagePerTick">틱당 데미지</param>
        /// <param name="duration">지속 시간</param>
        /// <param name="tickInterval">틱 간격</param>
        public PoisonModifier(float damagePerTick, float duration, float tickInterval = 0.5f)
        {
            this.damagePerTick = damagePerTick;
            this.duration = duration;
            this.tickInterval = tickInterval;
        }

        public void OnHit(Bullet bullet, IDamageable target, LayerMask targetLayer)
        {
            TickDamageManager.Instance.Apply(target, damagePerTick, duration, tickInterval);
        }
    }
}
