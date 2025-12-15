using STH.Core;
using STH.Combat.Projectiles;

namespace STH.Combat.Modifiers
{
    /// <summary>
    /// 관통 모디파이어 - 총알이 적을 관통
    /// </summary>
    public class PierceModifier : IBulletModifier
    {
        private readonly int pierceCount;

        /// <summary>
        /// 관통 모디파이어를 생성합니다.
        /// </summary>
        /// <param name="pierceCount">관통 횟수</param>
        public PierceModifier(int pierceCount)
        {
            this.pierceCount = pierceCount;
        }

        public void OnHit(Bullet bullet, IDamageable target)
        {
            // 총알의 관통 카운트 증가 (Bullet에서 차감됨)
            bullet.PierceCount += pierceCount;
        }
    }
}
