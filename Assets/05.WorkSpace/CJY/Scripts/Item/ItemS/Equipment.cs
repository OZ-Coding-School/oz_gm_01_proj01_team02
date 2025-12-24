using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
