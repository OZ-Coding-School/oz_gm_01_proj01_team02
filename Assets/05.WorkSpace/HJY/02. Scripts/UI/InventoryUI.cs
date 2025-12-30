
using System.Collections.Generic;
using UnityEngine;


public class InventoryUI : MonoBehaviour
{
    [Header("Scroll List")]
    public EquipmentSlotUI slotPrefab;
    public Transform content;

    private List<EquipmentSlotUI> slots = new();


    private void OnEnable()
    {
        if (InventoryManager.Instance != null)
        {
            // 이벤트 구독
            InventoryManager.Instance.OnEquipmentChanged += Refresh;
            Refresh();
        }
    }

    private void OnDisable()
    {
        if (InventoryManager.Instance != null)
        {
            // 이벤트 해제
            InventoryManager.Instance.OnEquipmentChanged -= Refresh;
        }
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
}

