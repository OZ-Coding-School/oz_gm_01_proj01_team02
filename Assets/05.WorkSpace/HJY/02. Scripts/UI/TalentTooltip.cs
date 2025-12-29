
using UnityEngine;
using TMPro;

public class TalentTooltip : MonoBehaviour
{
    public TextMeshProUGUI descriptionText;

    public void Show(string text)
    {
        descriptionText.text = text;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}