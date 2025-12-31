
using System.Collections.Generic;
using UnityEngine;

public class EquipmentDatabase : MonoBehaviour
{
    public static EquipmentDatabase Instance;

    [Header("All Equipments")]
    public List<EquipmentData> allEquipments = new();

    private void Awake()
    {
        Instance = this;
    }

    public EquipmentData GetEquipmentByName(string name)
    {
        return allEquipments.Find(e => e.equipmentName == name);
    }
}