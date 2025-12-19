using System;
using UnityEngine;

namespace STH.Core.Stats
{
    [Serializable]
    public class Stat
    {
        // 인스펙터에서 볼 기본값 (예: 공격력 100)
        [SerializeField] private float baseValue;

        // 합연산 퍼센트 (예: +10%, +20% -> 0.3)
        private float percentAddSum = 0f;

        // 값 변경 시 재계산 여부를 체크하는 Dirty Flag (최적화 핵심)
        private bool isDirty = true;
        private float cachedValue;

        public Stat(float baseValue)
        {
            this.baseValue = baseValue;
        }

        // 외부에서는 이 프로퍼티만 부르면 됨 (자동 계산)
        public float Value
        {
            get
            {
                if (isDirty)
                {
                    // 공식: 기본값 * (1 + 합연산 퍼센트)
                    cachedValue = baseValue * (1.0f + percentAddSum);
                    isDirty = false;
                }
                return cachedValue;
            }
        }

        // 스킬 획득 시 호출 (예: 10% 증가면 0.1f 전달)
        public void AddModifier(float amount)
        {
            if (amount != 0)
            {
                percentAddSum += amount;
                isDirty = true; // 값이 변했으니 다음 조회 때 재계산해라
            }
        }
    }
}
