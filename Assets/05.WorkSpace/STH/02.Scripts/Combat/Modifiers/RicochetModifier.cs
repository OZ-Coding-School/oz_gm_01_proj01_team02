using UnityEngine;
using STH.Core;
using STH.Combat.Projectiles;
using STH.Characters.Player;

namespace STH.Combat.Modifiers
{
    /// <summary>
    /// 리코셰 모디파이어 - 총알이 다른 적에게 튕김
    /// 각 Bullet마다 독립적인 상태(remainingBounces)를 가집니다.
    /// </summary>
    public class RicochetModifier : IStatefulModifier
    {
        private readonly int bounceCount;
        private readonly float searchRadius;
        private int remainingBounces;
        private readonly float attackMultiplier = -0.35f;

        /// <summary>
        /// 리코셰 모디파이어를 생성합니다.
        /// </summary>
        /// <param name="bounceCount">튕기는 횟수</param>
        /// <param name="searchRadius">다음 타겟 탐색 범위</param>
        public RicochetModifier(int bounceCount, float searchRadius = 10f, float attackMultiplier = -0.35f)
        {
            this.bounceCount = bounceCount;
            this.searchRadius = searchRadius;
            this.remainingBounces = bounceCount;
            this.attackMultiplier = attackMultiplier;
        }

        /// <summary>
        /// 새 Bullet을 위한 독립적인 인스턴스를 생성합니다.
        /// </summary>
        public IBulletModifier CreateInstance()
        {
            return new RicochetModifier(bounceCount, searchRadius);
        }

        public void OnHit(Bullet bullet, IDamageable target, LayerMask targetLayer)
        {
            Debug.Log($"Ricochet OnHit called - Remaining Bounces: {remainingBounces}");
            if (remainingBounces <= 0)
            {
                return; // 더 이상 튕기지 않음
            }
            // 다음 타겟 찾기
            Transform nextTarget = FindNextTarget(bullet.transform.position, target, targetLayer);
            if (nextTarget != null)
            {
                Vector3 direction = (nextTarget.position - bullet.transform.position).normalized;
                direction.y = 0;
                bullet.SetDirection(direction);
                bullet.PierceCount++; // 튕길 때는 파괴되지 않도록
                remainingBounces--;

                bullet.SetDamageWithCritical(bullet.damage + bullet.damage * attackMultiplier); // 치명타 다시 계산
            }
        }

        private Transform FindNextTarget(Vector3 position, IDamageable excludeTarget, LayerMask targetLayer)
        {
            Collider[] colliders = Physics.OverlapSphere(position, searchRadius, targetLayer);
            Transform nearestTarget = null;
            float nearestDistance = float.MaxValue;

            foreach (var collider in colliders)
            {
                IDamageable damageable = collider.GetComponent<IDamageable>();
                if (damageable == null || damageable == excludeTarget) continue;

                float distance = Vector3.Distance(position, collider.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestTarget = collider.transform;
                }
            }

            return nearestTarget;
        }
    }
}
