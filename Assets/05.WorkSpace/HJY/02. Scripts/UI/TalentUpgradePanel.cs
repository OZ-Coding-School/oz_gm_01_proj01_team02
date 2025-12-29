
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TalentUpgradePanel : MonoBehaviour, IPointerClickHandler
{
    public Image talentIcon;
    public TextMeshProUGUI talentNameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI prevLevelText;
    public TextMeshProUGUI currentLevelText;

    public void Show(TalentData data, int prevLevel, int currentLevel)
    {
        talentIcon.sprite = data.icon;
        talentNameText.text = data.talentName;
        descriptionText.text = data.description;
        prevLevelText.text = $"Lv.{prevLevel}";
        currentLevelText.text = $"Lv.{currentLevel}";

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    // ÅÇÇÏ¸é ´Ý±â
    public void OnPointerClick(PointerEventData eventData)
    {
        Hide();
    }
}