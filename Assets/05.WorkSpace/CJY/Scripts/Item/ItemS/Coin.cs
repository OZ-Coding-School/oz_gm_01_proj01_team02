using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : ItemBase
{
    [SerializeField]
    float rotationSpeedX, rotationSpeedY, rotationSpeedZ;

    void Update()
    {
        transform.Rotate(rotationSpeedX, rotationSpeedY, rotationSpeedZ);
    }

    public override void ReturnPool()
    {
        TestGameManager.Instance.GetExp(10);
        TestGameManager.Instance.GetCoin(10);
        if (PoolManager.pool_instance != null) PoolManager.pool_instance.ReturnPool(this);
    }
}
