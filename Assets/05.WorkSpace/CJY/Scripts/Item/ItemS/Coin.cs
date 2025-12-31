using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : ItemBase
{
    [SerializeField]
    float rotationSpeedX, rotationSpeedY, rotationSpeedZ;

    // public AudioClip coinSound;


    void Update()
    {
        transform.Rotate(rotationSpeedX, rotationSpeedY, rotationSpeedZ);
    }


    public override void ReturnPool()
    {
        TestGameManager.Instance.GetExp(10);
        TestGameManager.Instance.GetCoin(10);

        string cleanName = this.name.Replace("(Clone)", "").Trim();

        if (!GameManager.Data.collectedItemName.Contains(cleanName))
        {
            GameManager.Data.collectedItemName.Add(cleanName);
        }

        if (!GameManager.Data.collectedItemName.Contains("exp"))
        {
            GameManager.Data.collectedItemName.Add("exp");
        }

        GameManager.Data.collectedItem[cleanName] = TestGameManager.Instance.coin;
        GameManager.Data.collectedItem["exp"] = Mathf.FloorToInt(TestGameManager.Instance.coin * 0.1f);

        // Vector3 playPosition = Camera.main.transform.position;

        // AudioSource.PlayClipAtPoint(coinSound, playPosition);
        if (PoolManager.pool_instance != null) PoolManager.pool_instance.ReturnPool(this);
    }
}
