
using System.Collections.Generic;
using UnityEngine;

public class TalentManager : MonoBehaviour
{
    public static TalentManager Instance;

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

    // ���׷��̵� ���
    public int GetUpgradeCost(PermanentTalent talent)
    {
        return talent.baseCost + talent.level * talent.costIncrease;
    }

    // ��� ���׷��̵�
    public bool UpgradeTalent(string talentId)
    {
        PermanentTalent talent = talents.Find(t => t.talentId == talentId);
        if (talent == null) return false;
        if (talent.level >= talent.maxLevel) return false;

        int cost = GetUpgradeCost(talent);

        if (!CoinManager.Instance.UseCoin(cost))
            return false;

        talent.level++;
        SaveTalents();

        ApplyTalentEffect(talent);
        return true;
    }

    // ��� ȿ�� ����
    private void ApplyTalentEffect(PermanentTalent talent)
    {
        switch (talent.talentId)
        {
            case "Attack":
                PlayerStatManager.Instance.permanentAttackBonus =
                    talent.level * 2;
                break;

            case "HP":
                // // PlayerStatManager.Instance.permanentHpBonus =
                //     talent.level * 20;
                break;
        }

        PlayerStatManager.Instance.RecalculateStats();
    }

    // ����
    private void SaveTalents()
    {
        foreach (var t in talents)
        {
            PlayerPrefs.SetInt($"Talent_{t.talentId}", t.level);
        }
        PlayerPrefs.Save();
    }

    // �ε�
    private void LoadTalents()
    {
        foreach (var t in talents)
        {
            t.level = PlayerPrefs.GetInt($"Talent_{t.talentId}", 0);
            ApplyTalentEffect(t);
        }
    }
}