
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentDetailPanel : MonoBehaviour
{
    public static EquipmentDetailPanel Instance;

    [Header("UI References")]
    public TextMeshProUGUI nameText;                // 장비 이름
    public TextMeshProUGUI descriptionText;         // 장비 설명
    public TextMeshProUGUI abilityDescriptionText;  // 효과 설명

    public Button actionButton;
    public TextMeshProUGUI buttonText;

    // 장비 설명 패널 닫기
    public Button closeBtn;

    private EquipmentData current;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);

        // X버튼을 누르면
        closeBtn.onClick.AddListener(Close);
    }

    // 설명 패널을 닫아라
    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Show(EquipmentData data)
    {
        current = data;

        // 한글로 된 장비 이름
        nameText.text = data.displayName;

        // 설명 텍스트
        descriptionText.text = data.description;
        abilityDescriptionText.text = data.abilityDescription;

        RefreshButton();
        gameObject.SetActive(true);
    }

    private void RefreshButton()
    {
        actionButton.onClick.RemoveAllListeners();

        if (InventoryManager.Instance.IsEquipped(current))
        {
            buttonText.text = "해제"; // 혹은 장비 해제로 단어 대체
            actionButton.onClick.AddListener(Unequip);
        }
        else
        {
            buttonText.text = "착용"; // 혹은 장비로 단어 대체
            actionButton.onClick.AddListener(Equip);
        }
    }

    private void Equip()
    {
        InventoryManager.Instance.EquipItem(current);
        AfterChange();
    }

    private void Unequip()
    {
        InventoryManager.Instance.UnequipItem(current);
        AfterChange();
    }

    private void AfterChange()
    {
        // 플레이어 스탯 갱신
        FindObjectOfType<PlayerStatUI>()?.Refresh();

        // 내 컬렉션 UI 갱신
        FindObjectOfType<InventoryUI>()?.RefreshAll();

        // 버튼 갱신
        RefreshButton();
    }
}

