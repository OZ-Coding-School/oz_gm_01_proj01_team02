using UnityEngine;

namespace STH.Core
{
    /// <summary>
    /// 총알 능력 인터페이스 - 피격 시 효과의 규약
    /// </summary>
    public interface IBulletModifier
    {
        /// <summary>
        /// 총알 초기화 시 호출됩니다. (기본 구현: 아무것도 하지 않음)
        /// </summary>
        /// <param name="bullet">총알 자신</param>
        void OnInitialize(Combat.Projectiles.Bullet bullet) { }

        /// <summary>
        /// 총알이 타겟에 맞았을 때 호출됩니다.
        /// </summary>
        /// <param name="bullet">총알 자신</param>
        /// <param name="target">피격 대상</param>
        void OnHit(Combat.Projectiles.Bullet bullet, IDamageable target, LayerMask targetLayer);
    }

    /// <summary>
    /// 상태를 가진 Modifier를 위한 인터페이스
    /// 각 Bullet마다 독립적인 상태가 필요한 경우 구현합니다.
    /// </summary>
    public interface IStatefulModifier : IBulletModifier
    {
        /// <summary>
        /// 새로운 Bullet을 위한 독립적인 인스턴스를 생성합니다.
        /// </summary>
        IBulletModifier CreateInstance();
    }
}
