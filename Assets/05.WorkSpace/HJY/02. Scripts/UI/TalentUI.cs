
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TalentUI : MonoBehaviour
{
    // 재능 버튼들
    [Header("Talent Buttons")]
    public Button[] talentButtons; // 버튼 직접 연결하기

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

    [Header("Data")]
    public PlayerData playerData;   // PlayerData 참조

    private int currentCost = 100;
    private int upgradeCount = 0;
    private int currentTooltipIndex = -1;


    private void OnEnable()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnTalentChanged += RefreshTooltip;
        }
            
    }

    private void OnDisable()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnTalentChanged -= RefreshTooltip;
        }
            
    }

    private void Start()
    {
        // 버튼 클릭 이벤트 연결
        for (int i = 0; i < talentButtons.Length; i++)
        {
            int index = i; // 안전 코드 
            talentButtons[i].onClick.AddListener(() => OnClickTalent(index));
        }

        // 초기화
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

        tooltip.Show(
            data.talentName,              // 재능 이름
            talent.level,                 // 현재 레벨
            data.maxLevel,                // 최대 레벨
            data.talentType,              // 재능 타입 
            data.description,             // 재능 설명
            GetStatText(data, talent.level) // 스탯 텍스트
        );

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
        int cost = currentCost;

        // 코인 사용 시도
        if (!playerData.UseCoin(cost))
        {
            shortagePanel.Open();
            return;
        }


        CoinManager.Instance.SaveCoin(playerData); // 저장
        FindObjectOfType<TitleManager>()?.UpdateUI();



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

        // UI 갱신
        FindObjectOfType<TitleManager>()?.UpdateUI();

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

