
using System.Collections.Generic;
using UnityEngine;

public class TalentManager : MonoBehaviour
{
    public static TalentManager Instance;

    [Header("Data")]
    public PlayerData playerData; // PlayerData 참조 추가

    [System.Serializable]
    public class PermanentTalent
    {
        public string talentId;
        public int level;
        public int maxLevel;
        public int baseCost;
        public int costIncrease;
    }

    public List<PermanentTalent> talents;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadTalents();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 업그레이드 비용 계산
    public int GetUpgradeCost(PermanentTalent talent)
    {
        return talent.baseCost + talent.level * talent.costIncrease;
    }

    // 재능 업그레이드
    public bool UpgradeTalent(string talentId)
    {
        PermanentTalent talent = talents.Find(t => t.talentId == talentId);
        if (talent == null) return false;
        if (talent.level >= talent.maxLevel) return false;

        int cost = GetUpgradeCost(talent);

        // PlayerData에서 코인 차감
        if (playerData == null) return false;
        if (!playerData.UseCoin(cost))
            return false;

        // 차감된 코인 저장
        if (CoinManager.Instance != null)
            CoinManager.Instance.SaveCoin(playerData);

        talent.level++;
        SaveTalents();

        ApplyTalentEffect(talent);

        // 필요 시 UI 갱신 (타이틀 씬 한정)
        FindObjectOfType<TitleManager>()?.UpdateUI();

        return true;

    }

    // 재능 효과 적용
    private void ApplyTalentEffect(PermanentTalent talent)
    {
        switch (talent.talentId)
        {
            case "Attack":
                PlayerStatManager.Instance.permanentAttackBonus = talent.level * 2;
                break;

            case "HP":
                // PlayerStatManager.Instance.permanentHpBonus = talent.level * 20;
                break;
        }

        PlayerStatManager.Instance.RecalculateStats();
    }

    // 저장
    private void SaveTalents()
    {
        foreach (var t in talents)
        {
            PlayerPrefs.SetInt($"Talent_{t.talentId}", t.level);
        }
        PlayerPrefs.Save();
    }

    // 실행
    private void LoadTalents()
    {
        foreach (var t in talents)
        {
            t.level = PlayerPrefs.GetInt($"Talent_{t.talentId}", 0);
            ApplyTalentEffect(t);
        }
    }
}