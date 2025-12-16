using System.Collections.Generic;
using UnityEngine;
using STH.Core;



namespace STH.Systems
{
    public class TickDamageManager : MonoBehaviour
    {
        public static TickDamageManager Instance { get; private set; }

        // 틱 데미지 정보를 담는 구조체
        private struct TickDamageData
        {
            public IDamageable Target;
            public float DamagePerTick;
            public float TickInterval;
            public float RemainingDuration;
            public float TickTimer;
        }

        private List<TickDamageData> activeEffects = new List<TickDamageData>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Update()
        {
            for (int i = activeEffects.Count - 1; i >= 0; i--)
            {
                var effect = activeEffects[i];

                effect.RemainingDuration -= Time.deltaTime;
                effect.TickTimer += Time.deltaTime;

                // 틱 타이밍이면 데미지
                if (effect.TickTimer >= effect.TickInterval)
                {
                    effect.TickTimer = 0f;
                    effect.Target?.TakeDamage(effect.DamagePerTick);
                }

                // 끝났으면 제거, 아니면 갱신
                if (effect.RemainingDuration <= 0)
                    activeEffects.RemoveAt(i);
                else
                    activeEffects[i] = effect;
            }
        }

        /// <summary>
        /// 틱 데미지를 적용합니다.
        /// </summary>
        public void Apply(IDamageable target, float damagePerTick, float duration, float tickInterval = 0.5f)
        {
            activeEffects.Add(new TickDamageData
            {
                Target = target,
                DamagePerTick = damagePerTick,
                RemainingDuration = duration,
                TickInterval = tickInterval,
                TickTimer = tickInterval
            });
        }

        /// <summary>
        /// 특정 타겟의 모든 틱 데미지를 제거합니다.
        /// </summary>
        public void RemoveAll(IDamageable target)
        {
            activeEffects.RemoveAll(e => e.Target == target);
        }

        /// <summary>
        /// 모든 틱 데미지를 제거합니다.
        /// </summary>
        public void Clear()
        {
            activeEffects.Clear();
        }
    }
}