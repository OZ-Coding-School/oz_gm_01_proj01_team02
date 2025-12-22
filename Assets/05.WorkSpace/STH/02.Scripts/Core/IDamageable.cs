namespace STH.Core
{
    /// <summary>
    /// 피격 인터페이스 - 데미지를 받을 수 있는 객체의 규약
    /// </summary>
    public interface IDamageable
    {
        /// <summary>
        /// 데미지를 받습니다.
        /// </summary>
        /// <param name="amount">데미지량</param>
        void TakeDamage(float amount);

        /// <summary>
        /// 사망 처리를 합니다.
        /// </summary>
        void Die();

    }
}
