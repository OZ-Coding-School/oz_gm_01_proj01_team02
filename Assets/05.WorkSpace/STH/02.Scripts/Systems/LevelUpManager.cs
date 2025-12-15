using System.Collections.Generic;
using UnityEngine;
using STH.ScriptableObjects.Base;
using STH.Characters.Player;

namespace STH.Systems
{
    /// <summary>
    /// 레벨업 매니저 - 스킬 뽑기 및 적용 UI 관리
    /// </summary>
    public class LevelUpManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerController player;
        [SerializeField] private List<SkillData> allSkills = new List<SkillData>();

        [Header("Settings")]
        [SerializeField] private int optionsCount = 3;

        public System.Action<List<SkillData>> OnShowOptions;
        public System.Action OnHideOptions;

        /// <summary>
        /// 레벨업 옵션을 표시합니다.
        /// </summary>
        public void ShowLevelUpOptions()
        {
            List<SkillData> options = GetRandomSkills(optionsCount);
            OnShowOptions?.Invoke(options);
            Time.timeScale = 0f; // 게임 일시정지
        }

        /// <summary>
        /// 스킬을 선택했을 때 호출됩니다.
        /// </summary>
        public void OnSkillSelected(SkillData skill)
        {
            if (skill != null && player != null)
            {
                skill.Apply(player);
            }
            OnHideOptions?.Invoke();
            Time.timeScale = 1f; // 게임 재개
        }

        /// <summary>
        /// 랜덤 스킬을 가져옵니다.
        /// </summary>
        private List<SkillData> GetRandomSkills(int count)
        {
            List<SkillData> result = new List<SkillData>();
            List<SkillData> available = new List<SkillData>(allSkills);

            for (int i = 0; i < count && available.Count > 0; i++)
            {
                int randomIndex = Random.Range(0, available.Count);
                result.Add(available[randomIndex]);
                available.RemoveAt(randomIndex);
            }

            return result;
        }

        /// <summary>
        /// 스킬 풀에 스킬을 추가합니다.
        /// </summary>
        public void AddSkillToPool(SkillData skill)
        {
            if (!allSkills.Contains(skill))
            {
                allSkills.Add(skill);
            }
        }

        /// <summary>
        /// 스킬 풀에서 스킬을 제거합니다.
        /// </summary>
        public void RemoveSkillFromPool(SkillData skill)
        {
            allSkills.Remove(skill);
        }
    }
}
