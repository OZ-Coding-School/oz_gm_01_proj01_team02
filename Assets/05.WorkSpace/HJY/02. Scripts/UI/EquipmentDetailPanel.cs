
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentDetailPanel : MonoBehaviour
{
    public static EquipmentDetailPanel Instance;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI nameText;                // 장비 이름
    [SerializeField] private TextMeshProUGUI descriptionText;         // 장비 설명
    [SerializeField] private TextMeshProUGUI abilityDescriptionText;  // 효과 설명

    [SerializeField] private Button actionButton;
    [SerializeField] private TextMeshProUGUI buttonText;

    [SerializeField] private Button closeBtn;

    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private PlayerStatUI playerStatUI;

    private EquipmentData current;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);

        // X 버튼을 누르면 닫아라
        closeBtn.onClick.AddListener(Close);
    }

    // 설명 패널 닫기
    public void Close()
    {
        gameObject.SetActive(false);
    }

    // 설명 패널 열기
    public void Show(EquipmentData data)
    {
        current = data;

        nameText.text = data.displayName; // 한글로 된 장비 설명
        descriptionText.text = data.description; // 장비 설명 텍스트
        abilityDescriptionText.text = data.abilityDescription; // 장비 효과 텍스트

        RefreshButton();
        gameObject.SetActive(true);
    }

    private void RefreshButton()
    {
        actionButton.onClick.RemoveAllListeners();

        if (InventoryManager.Instance.IsEquipped(current))
        {
            buttonText.text = "해제";
            actionButton.onClick.AddListener(Unequip);
        }
        else
        {
            buttonText.text = "착용";
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
        // 플레이어 스탯 UI 갱신
        playerStatUI?.Refresh();

        // 내 컬렉션 UI 갱신
        inventoryUI?.RefreshAll();

        // 버튼 상태 갱신
        RefreshButton();
    }
}