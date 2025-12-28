using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Equipment : ItemBase
{
    public EquipmentData equipmentData; // ScriptableObject 참조

    public override void ReturnPool()
    {
        if (!GameManager.Data.collectedItemName.Contains(this.name))
        {
            GameManager.Data.collectedItemName.Add(this.name);
        }

        if (GameManager.Data.collectedItem.ContainsKey(this.name))
        {
            GameManager.Data.collectedItem[this.name] += 1;
        }
        else GameManager.Data.collectedItem.Add(this.name, 1);

        // 인벤토리에 추가
        if (equipmentData != null)
        {
            InventoryManager.Instance.AddEquipment(equipmentData);
        }

        if (PoolManager.pool_instance != null) PoolManager.pool_instance.ReturnPool(this);
    }
}



/*
 
// 원래 코드
public class Equipment : ItemBase
{

    public override void ReturnPool()
    {
        if (!GameManager.Data.collectedItemName.Contains(this.name))
        {
            GameManager.Data.collectedItemName.Add(this.name);
        }

        if (GameManager.Data.collectedItem.ContainsKey(this.name))
        {
            GameManager.Data.collectedItem[this.name] += 1;
        }
        else GameManager.Data.collectedItem.Add(this.name, 1);
        if (PoolManager.pool_instance != null) PoolManager.pool_instance.ReturnPool(this);
    }
}

*/
