
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentSlotUI: MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI statText;
    public GameObject equippedMark;
    
    private EquipmentData equipment;
    private InventoryUI inventoryUI;

    private void Awake()
    {
        inventoryUI = GetComponentInParent<InventoryUI>();
    }

    public void SetEquipment(EquipmentData data)
    {
        equipment = data;
        iconImage.sprite = equipment.icon;
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (equipment == null) return;

        nameText.text = equipment.equipmentName;
        statText.text = GetStatText();

        int index = (int)equipment.type;
        equippedMark.SetActive(InventoryManager.Instance.equippedItems[index] == equipment);
    }

    private string GetStatText()
    {
        string result = "";
        foreach (var stat in equipment.stats)
        {
            result += $"{stat.statType} +{stat.value}\n";
        }
        return result;
    }

    public void OnClickSlot()
    {
        EquipmentDetailPanel.Instance.Show(equipment);
    }
}

