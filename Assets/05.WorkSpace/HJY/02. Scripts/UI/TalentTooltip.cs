
using UnityEngine;
using TMPro;

public class TalentTooltip : MonoBehaviour
{
    [Header("Texts")]
    public TextMeshProUGUI nameText;        // 재능 이름
    public TextMeshProUGUI levelText;       // 현재 레벨
    public TextMeshProUGUI descriptionText; // 재능 설명

   

    public void Show(string name, int level, int maxLevel, TalentType type, string description)
    {
        // 이름 표시
        nameText.text = name;

        // 레벨 표시 (Max 레벨 조건)
        levelText.text = level >= maxLevel ? "Max" : $"Lv.{level}";


        // 설명 표시
        descriptionText.text = $"{description}";

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}