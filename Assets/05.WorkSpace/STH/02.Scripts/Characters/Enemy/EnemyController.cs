using UnityEngine;
using STH.Core;
using STH.Combat.Projectiles;
using STH.ScriptableObjects.Base;

namespace STH.Characters.Enemy
{
    /// <summary>
    /// 적 컨트롤러 - SO에서 전략 하나를 받아와서 사용
    /// </summary>
    public class EnemyController : MonoBehaviour, IDamageable
    {
        public void TakeDamage(float amount)
        {

        }

        public void Die()
        {

        }
    }
}
