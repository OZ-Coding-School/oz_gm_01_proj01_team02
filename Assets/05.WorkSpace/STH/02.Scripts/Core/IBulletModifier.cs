namespace STH.Core
{
    /// <summary>
    /// 총알 능력 인터페이스 - 피격 시 효과의 규약
    /// </summary>
    public interface IBulletModifier
    {
        /// <summary>
        /// 총알이 타겟에 맞았을 때 호출됩니다.
        /// </summary>
        /// <param name="bullet">총알 자신</param>
        /// <param name="target">피격 대상</param>
        void OnHit(Combat.Projectiles.Bullet bullet, IDamageable target);
    }
}
