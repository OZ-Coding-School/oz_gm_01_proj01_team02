
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class EquipmentDetailPanel : MonoBehaviour
{
    public static EquipmentDetailPanel Instance;

    public TextMeshProUGUI nameText;
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
        nameText.text = data.equipmentName;

        RefreshButton();
        gameObject.SetActive(true);
    }

    private void RefreshButton()
    {
        actionButton.onClick.RemoveAllListeners();

        if (InventoryManager.Instance.IsEquipped(current))
        {
            buttonText.text = "장비 해제"; // 혹은 해제로 단어 대체
            actionButton.onClick.AddListener(Unequip);
        }
        else
        {
            buttonText.text = "장비"; // 혹은 착용으로 단어 대체
            actionButton.onClick.AddListener(Equip);
            buttonText.text = "장비";
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

