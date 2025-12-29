
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TalentUI : MonoBehaviour
{
    // 재능 설명 말풍선
    [Header("Tooltip")]
    public TalentTooltip tooltip;

    // 재능 업그레이드
    [Header("Upgrade UI")]
    public Button upgradeButton;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI upgradeCountText;

    // 패널들
    [Header("Panels")]
    public ShortagePanel shortagePanel;      // 코인 부족 패널
    public TalentUpgradePanel upgradePanel;  // 업그레이드 결과 패널

    private int currentCost = 100;
    private int upgradeCount = 0;
    private int currentTooltipIndex = -1;

    private void OnEnable()
    {
        InventoryManager.Instance.OnTalentChanged += RefreshTooltip;
    }

    private void OnDisable()
    {
        InventoryManager.Instance.OnTalentChanged -= RefreshTooltip;
    }

    private void Start()
    {
        tooltip.Hide();
        shortagePanel.Close();
        upgradePanel.Hide();

        if (upgradeButton != null)
        {
            upgradeButton.onClick.AddListener(OnClickUpgrade);
        }
        
        RefreshUpgradeUI();
    }

    // ===================== 재능 클릭 =====================

    public void OnClickTalent(int index)
    {
        List<TalentState> talents = InventoryManager.Instance.ownedTalents;
        if (index >= talents.Count) return;

        if (currentTooltipIndex == index && tooltip.gameObject.activeSelf)
        {
            tooltip.Hide();
            currentTooltipIndex = -1;
            return;
        }

        ShowTooltip(index);
    }

    private void ShowTooltip(int index)
    {
        TalentState talent = InventoryManager.Instance.ownedTalents[index];
        TalentData data = talent.data;

        tooltip.Show($"{data.talentName}\n" + $"레벨 {talent.level}/{data.maxLevel}\n\n" + $"{data.description}\n" + GetStatText(data, talent.level));

        currentTooltipIndex = index;
    }

    private void RefreshTooltip()
    {
        if (currentTooltipIndex < 0) return;
        ShowTooltip(currentTooltipIndex);
    }

    // ===================== 업그레이드 =====================

    private void OnClickUpgrade()
    {
        // 코인 체크
        int cost = currentCost;
        if (!CoinManager.Instance.UseCoin(currentCost))
        {
            shortagePanel.Open();
            return;
        }


        // 업그레이드 가능한 재능 필터
        List<TalentState> candidates = new();

        foreach (TalentState talent in InventoryManager.Instance.ownedTalents)
        {
            if (talent.data.talentType == TalentType.StartSkillMax)
                continue;

            if (!talent.IsMaxLevel)
                candidates.Add(talent);
        }

        if (candidates.Count == 0) return;

        TalentState chosen = candidates[Random.Range(0, candidates.Count)];

        int prevLevel = chosen.level;
        chosen.level++;

        PlayerStatManager.Instance.RecalculateStats();
        

        upgradeCount++;
        currentCost += 100;
        RefreshUpgradeUI();

        upgradePanel.Show(chosen.data, prevLevel, chosen.level);

        // 말풍선 갱신
        if (currentTooltipIndex >= 0)
        {
            ShowTooltip(currentTooltipIndex);
        }
    }

    // ===================== UI =====================

    

   private void RefreshUpgradeUI()
   {
       costText.text = $"X {currentCost}";
       upgradeCountText.text = $"{upgradeCount}회 업그레이드";
   }

    private string GetStatText(TalentData data, int level)
    {
        StatValue stat = data.statPerLevel;

        if (stat.statType == StatType.None)
            return "특수 효과만 생김";

        float value = stat.value * level;
        return $"{stat.statType} +{value}";
    }
}

