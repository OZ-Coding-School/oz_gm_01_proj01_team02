
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class TalentUI : MonoBehaviour
{
    [Header("UI References")]
    public TalentTooltip tooltip;             // 말풍선 Prefab 참조인데 만들어야 함
    // public Button drawButton;                 // 재능 뽑기 버튼인데 테스트용임
    public Button upgradeButton;              // 업그레이드 버튼
    public TextMeshProUGUI costText;          // 업그레이드 비용 표시
    public TextMeshProUGUI upgradeCountText;  // 업그레이드 횟수 표시

    private int upgradeCount = 0;
    private int currentCost = 0;

    private void Start()
    {
        // drawButton.onClick.AddListener(OnClickTalentDraw);
        upgradeButton.onClick.AddListener(OnClickUpgrade);

        // tooltip.Hide(); // 시작 시 말풍선 숨김
        RefreshUpgradeUI();
    }

    // 재능 뽑기 버튼 클릭 시 호출
    private void OnClickTalentDraw()
    {
        InventoryManager.Instance.AcquireTalent();

        // 가장 최근에 획득한 재능
        TalentState lastTalent = InventoryManager.Instance.ownedTalents[
            InventoryManager.Instance.ownedTalents.Count - 1];

        TalentData data = lastTalent.data;

        // 첫 비용 설정
        currentCost = data.baseCost;

        // 말풍선에 설명 표시
        tooltip.Show($"{data.talentName}\n{data.description}\n" + $"레벨 {lastTalent.level}/{data.maxLevel}\n" + GetStatText(data, lastTalent.level));
    }

    // 업그레이드 버튼 클릭 시 호출
    private void OnClickUpgrade()
    {
        List<TalentState> talents = InventoryManager.Instance.ownedTalents;
        if (talents.Count == 0) return;

        // 랜덤 재능 선택
        int randomIndex = Random.Range(0, talents.Count);
        TalentState chosen = talents[randomIndex];
        TalentData data = chosen.data;

        // 최대 레벨 체크
        if (chosen.level < data.maxLevel)
        {
            chosen.level++;
            PlayerStatManager.Instance.RecalculateStats();

            upgradeCount++;
            currentCost += data.costIncrease;
            RefreshUpgradeUI();

            // 재능 설명 업데이트
            tooltip.Show($"{data.talentName}\n{data.description}\n" +
                         $"레벨 {chosen.level}/{data.maxLevel}\n" +
                         GetStatText(data, chosen.level));
        }
        else
        {
            tooltip.Show($"{data.talentName}은 이미 최대 레벨입니다!");
        }
    }

  
    // 비용 및 횟수 UI 갱신
    private void RefreshUpgradeUI()
    {
        if (costText != null)
            costText.text = $"X {currentCost}";

        if (upgradeCountText != null)
            upgradeCountText.text = $"{upgradeCount}회 업그레이드";
    }

    // 스탯 텍스트 생성
    private string GetStatText(TalentData data, int level)
    {
        if (data.statPerLevel.statType != StatType.None)
        {
            float totalValue = data.statPerLevel.value * level;
            return $"{data.statPerLevel.statType} +{totalValue}";
        }
        else
        {
            return "특수 효과로 올라가는 스탯은 없습니다.";
        }
    }
}