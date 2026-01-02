using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : ItemBase
{
    PlayerHealth playerHP;
    protected override void OnEnable()
    {
        base.OnEnable();
        playerHP = player.GetComponent<PlayerHealth>();
    }

    public override void ReturnPool()
    {
        playerHP.Heal(100);
        if (PoolManager.pool_instance != null) PoolManager.pool_instance.ReturnPool(this);
    }
}
