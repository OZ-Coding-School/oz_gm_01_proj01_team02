
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("UI References")]
    public Transform content;               // 슬롯들이 들어갈 부모 오브젝트
    public EquipmentSlotUI slotPrefab;      // 슬롯 프리팹

    // 슬롯들을 접근을 위해 리스트로 관리
    private List<EquipmentSlotUI> slots = new List<EquipmentSlotUI>();

    private void OnEnable()
    {
        if (InventoryManager.Instance != null)
        {
            // 인벤토리 매니저 이벤트 구독
            InventoryManager.Instance.OnEquipmentChanged += Refresh;
            Refresh(); // 켜질 때 한 번 갱신
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

        // 가지고 있는 장비 표시
        foreach (var item in InventoryManager.Instance.ownedEquipments)
        {
            EquipmentSlotUI slot = Instantiate(slotPrefab, content);

            // 처음엔 꺼진 상태로 생성
            slot.gameObject.SetActive(false);

            // 장비 데이터가 있으면 켜기
            if (item != null)
            {
                slot.SetEquipment(item);          // 슬롯에 장비 데이터 연결
                slot.gameObject.SetActive(true);  // 슬롯 켜기
            }

            slots.Add(slot); // 리스트에 추가

        }
    }
}