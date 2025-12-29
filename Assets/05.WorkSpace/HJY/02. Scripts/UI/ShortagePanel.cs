
using UnityEngine;
using UnityEngine.UI;

public class ShortagePanel : MonoBehaviour
{
    [Header("UI")]
    public Button closeButton;

    private void Awake()
    {
        // 기본적으로 꺼진 상태
        gameObject.SetActive(false);

        // 패널 닫기
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(Close);
        }
            
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}

