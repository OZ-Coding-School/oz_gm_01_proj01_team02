
using UnityEngine;
using TMPro;

public class TalentTooltip : MonoBehaviour
{
    [Header("Texts")]
    public TextMeshProUGUI nameText;        // 재능 이름
    public TextMeshProUGUI levelText;       // 현재 레벨
    public TextMeshProUGUI descriptionText; // 재능 설명

    public void Show(string name, int level, int maxLevel, TalentType type, string description, string statText)
    {
        // 이름 표시
        nameText.text = name;

        // 레벨 표시 (Max 레벨 조건)
        if (level >= maxLevel || type == TalentType.StartSkillMax)
            levelText.text = "Max";
        else
            levelText.text = $"Lv.{level}";

        // 설명 및 스탯 표시
        descriptionText.text = $"{description}\n{statText}";

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}