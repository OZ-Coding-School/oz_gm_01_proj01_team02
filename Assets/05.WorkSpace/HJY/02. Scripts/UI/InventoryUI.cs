
using System.Collections.Generic;
using UnityEngine;


public class InventoryUI : MonoBehaviour
{
    [Header("Scroll List")]
    public EquipmentSlotUI slotPrefab;
    public Transform content;

    private List<EquipmentSlotUI> slots = new List<EquipmentSlotUI>();

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        // 기존 슬롯 제거
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        slots.Clear();

        foreach (var item in InventoryManager.Instance.ownedEquipments)
        {
            EquipmentSlotUI slot = Instantiate(slotPrefab, content);
            slot.SetEquipment(item);
            slots.Add(slot);
        }
    }

    public void RefreshAll()
    {
        foreach (var slot in slots)
        {
            slot.RefreshUI();
        } 
    }
}

