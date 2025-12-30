
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour
{
    public Image iconImage;   // 장비 아이콘만 표시

    private EquipmentData equipment;

    // 장비 데이터 세팅
    public void SetEquipment(EquipmentData data)
    {
        equipment = data;
        if (iconImage != null && equipment != null)
        {
            iconImage.sprite = equipment.icon; // 장비 데이터의 아이콘 표시
        }
    }

    // 슬롯 클릭 시 장비 설명 패널 열기
    public void OnClickSlot()
    {
        if (equipment != null)
        {
            EquipmentDetailPanel.Instance.Show(equipment);
        }
    }
}