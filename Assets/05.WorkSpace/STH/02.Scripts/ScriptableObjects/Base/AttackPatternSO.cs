using UnityEngine;
using STH.Core;

namespace STH.ScriptableObjects.Base
{
    /// <summary>
    /// 공격 패턴 SO 추상 클래스 - 적 패턴의 부모
    /// </summary>
    public abstract class AttackPatternSO : ScriptableObject
    {
        [Header("Projectile")]
        [SerializeField] private GameObject bulletPrefab;

        public GameObject BulletPrefab => bulletPrefab;

        /// <summary>
        /// 발사 전략을 생성합니다.
        /// </summary>
        public abstract IFireStrategy CreateStrategy();
    }
}
