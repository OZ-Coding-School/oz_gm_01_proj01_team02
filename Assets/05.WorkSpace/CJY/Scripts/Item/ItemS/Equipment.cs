using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : ItemBase
{
    public override void ReturnPool()
    {
        if (PoolManager.pool_instance != null) PoolManager.pool_instance.ReturnPool(this);
    }
}
