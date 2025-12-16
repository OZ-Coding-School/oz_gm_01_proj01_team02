
using UnityEngine;
using TMPro;

public class TalentUI : MonoBehaviour
{
    [Header("Optional UI")]
    public TextMeshProUGUI resultText;

    public void OnClickTalentDraw()
    {
        InventoryManager.Instance.AcquireTalent();

        if (resultText != null)
        {
            TalentData lastTalent =
                InventoryManager.Instance.ownedTalents[
                    InventoryManager.Instance.ownedTalents.Count - 1
                ];

            resultText.text = $"È¹µæ Àç´É: {lastTalent.talentName}\n" + $"{lastTalent.stat.statType} +{lastTalent.stat.value}";
        }
    }
}

