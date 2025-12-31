

using UnityEngine;
using UnityEngine.UI;

public class EquippedUIManager : MonoBehaviour
{
    public static EquippedUIManager Instance;

    // 장비 슬롯 4칸 이미지들
    [Header("Equipped Slot RawImages")]
    [SerializeField] private RawImage weaponSlotImage;
    [SerializeField] private RawImage armorSlotImage;
    [SerializeField] private RawImage ringSlotImage1;
    [SerializeField] private RawImage ringSlotImage2;

    // 장비 타입별 기본 이미지들
    [Header("Default Textures")]
    [SerializeField] private Texture2D defaultWeaponTexture;
    [SerializeField] private Texture2D defaultArmorTexture;
    [SerializeField] private Texture2D defaultRingTexture;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnEquipmentChanged += Refresh;
            Refresh();
        }
    }

    private void OnDisable()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnEquipmentChanged -= Refresh;
        }
    }

    public void Refresh()
    {
        var inv = InventoryManager.Instance;

        // 무기
        if (inv.equippedWeapon != null)
        {
            weaponSlotImage.texture = inv.equippedWeapon.icon.texture;
            AddClickEvent(weaponSlotImage, inv.equippedWeapon);
        }
        else
        {
            weaponSlotImage.texture = defaultWeaponTexture;
            RemoveClickEvent(weaponSlotImage);
        }

        // 방어구
        if (inv.equippedArmor != null)
        {
            armorSlotImage.texture = inv.equippedArmor.icon.texture;
            AddClickEvent(armorSlotImage, inv.equippedArmor);
        }
        else
        {
            armorSlotImage.texture = defaultArmorTexture;
            RemoveClickEvent(armorSlotImage);
        }

        // 반지1
        if (inv.equippedRings[0] != null)
        {
            ringSlotImage1.texture = inv.equippedRings[0].icon.texture;
            AddClickEvent(ringSlotImage1, inv.equippedRings[0]);
        }
        else
        {
            ringSlotImage1.texture = defaultRingTexture;
            RemoveClickEvent(ringSlotImage1);
        }

        // 반지2
        if (inv.equippedRings[1] != null)
        {
            ringSlotImage2.texture = inv.equippedRings[1].icon.texture;
            AddClickEvent(ringSlotImage2, inv.equippedRings[1]);
        }
        else
        {
            ringSlotImage2.texture = defaultRingTexture;
            RemoveClickEvent(ringSlotImage2);
        }
    }

    private void AddClickEvent(RawImage slotImage, EquipmentData data)
    {
        var btn = slotImage.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => EquipmentDetailPanel.Instance.Show(data));
        }
    }

    private void RemoveClickEvent(RawImage slotImage)
    {
        var btn = slotImage.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.RemoveAllListeners();
        }
    }
}