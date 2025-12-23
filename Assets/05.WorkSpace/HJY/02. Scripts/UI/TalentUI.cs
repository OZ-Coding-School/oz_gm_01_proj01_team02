
using UnityEngine;
using TMPro;

public class TalentUI : MonoBehaviour
{
    [Header("Optional UI")]
    public TextMeshProUGUI resultText;

    public void OnClickTalentDraw()
    {
        InventoryManager.Instance.AcquireTalent();

        if (resultText == null) return;

        // 가장 최근에 획득 및 업그레이드된 재능
        TalentState lastTalent = InventoryManager.Instance.ownedTalents[InventoryManager.Instance.ownedTalents.Count - 1];

        TalentData data = lastTalent.data;

        // 레벨 반영된 스탯 값 계산
        float totalValue = data.statPerLevel.value * lastTalent.level;

        string statText = data.statPerLevel.statType != 0 ? $"{data.statPerLevel.statType} +{totalValue}" : "특수 효과";

        resultText.text = $"획득 재능: {data.talentName}\n" + $"레벨: {lastTalent.level}/{data.maxLevel}\n" + statText;
    }
}
