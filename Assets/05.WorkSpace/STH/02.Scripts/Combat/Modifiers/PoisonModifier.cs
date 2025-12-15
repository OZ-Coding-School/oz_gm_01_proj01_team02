using UnityEngine;
using STH.Core;
using STH.Combat.Projectiles;

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

        public void OnHit(Bullet bullet, IDamageable target)
        {
            // 독 효과 적용 (MonoBehaviour가 필요하므로 타겟에서 처리)
            if (target is MonoBehaviour mono)
            {
                var poisonEffect = mono.gameObject.GetComponent<PoisonEffect>();
                if (poisonEffect == null)
                {
                    poisonEffect = mono.gameObject.AddComponent<PoisonEffect>();
                }
                poisonEffect.Apply(damagePerTick, duration, tickInterval);
            }
        }
    }

    /// <summary>
    /// 독 효과 컴포넌트 - 실제 DoT 처리
    /// </summary>
    public class PoisonEffect : MonoBehaviour
    {
        private float damagePerTick;
        private float remainingDuration;
        private float tickInterval;
        private float tickTimer;
        private IDamageable target;

        private void Awake()
        {
            target = GetComponent<IDamageable>();
        }

        public void Apply(float damage, float duration, float interval)
        {
            damagePerTick = damage;
            remainingDuration = duration;
            tickInterval = interval;
            tickTimer = 0f;
        }

        private void Update()
        {
            if (remainingDuration <= 0) return;

            remainingDuration -= Time.deltaTime;
            tickTimer += Time.deltaTime;

            if (tickTimer >= tickInterval)
            {
                tickTimer = 0f;
                target?.TakeDamage(damagePerTick);
            }

            if (remainingDuration <= 0)
            {
                Destroy(this);
            }
        }
    }
}
