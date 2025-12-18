using System;
using System.Collections.Generic;
using UnityEngine;

namespace STH.Core.Stats
{
    public enum StatType
    {
        Attack,
        MaxHP,
        AttackSpeed,
        MoveSpeed,  // 여기에 추가만 하면 끝!
        CritRate,
        CritDamage,
        SuperTime
    }

    [Serializable]
    public struct StatInitData
    {
        public StatType type;
        public float baseValue;
    }

    /// <summary>
    /// 게임 스탯 데이터 클래스
    /// </summary>
    public class CharacterStats : MonoBehaviour
    {
        // 스탯 타입별로 Stat 객체를 매핑
        private Dictionary<StatType, Stat> _stats;

        [Header("Initial Setup")] // 초기값 설정을 위한 단순 클래스 리스트 (인스펙터용)
        [SerializeField] private List<StatInitData> _initialStats;

        private float baseAttack = 150;
        private float baseMaxHp = 1000;
        private float baseAttackSpeed = 1;
        private float baseMoveSpeed = 5;
        private float baseCritRate = 0.05f;
        private float baseCritDamage = 2f;
        private float baseSuperTime = 0.5f;

        private void Awake()
        {
            _stats = new Dictionary<StatType, Stat>();
            InitializeStats();
        }

        private void InitializeStats()
        {
            // 1. 인스펙터에 설정된 초기값으로 딕셔너리 빌드
            if (new CharacterStats() != null)
            {
                foreach (var initData in _initialStats)
                {
                    _stats[initData.type] = new Stat(initData.baseValue);
                }
            }
            // 기본값 세팅
            else
            {
                _stats[StatType.Attack] = new Stat(baseAttack);
                _stats[StatType.MaxHP] = new Stat(baseMaxHp);
                _stats[StatType.AttackSpeed] = new Stat(baseAttackSpeed);
                _stats[StatType.MoveSpeed] = new Stat(baseMoveSpeed);
                _stats[StatType.CritRate] = new Stat(baseCritRate);
                _stats[StatType.CritDamage] = new Stat(baseCritDamage);
                _stats[StatType.SuperTime] = new Stat(baseSuperTime);
            }

            // 2. 만약 없는 스탯이 요청되면 기본값 0 등으로 생성 (방어 코드)
            foreach (StatType type in Enum.GetValues(typeof(StatType)))
            {
                if (!_stats.ContainsKey(type))
                    _stats[type] = new Stat(1f); // 혹은 1f 등 기본값
            }
        }

        // 외부에서 스탯 값을 가져오는 함수
        public float GetStat(StatType type)
        {
            return _stats[type].Value; // Stat 객체가 알아서 계산해서 줌
        }

        // 스킬 등으로 스탯을 올리는 함수 (변수 추가 필요 없음!)
        public void IncreaseStat(StatType type, float percentage)
        {
            if (_stats.ContainsKey(type))
            {
                _stats[type].AddModifier(percentage);
            }
        }
    }
}
