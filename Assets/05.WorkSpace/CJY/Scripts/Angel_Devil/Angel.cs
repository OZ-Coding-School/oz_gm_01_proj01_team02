using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angel : MonoBehaviour
{
    AngelUIManagers angelPanel;

    private void Awake()
    {
        angelPanel = FindObjectOfType<AngelUIManagers>(true);
    }
    public void ReturnPool()
    {
        if (PoolManager.pool_instance != null) PoolManager.pool_instance.ReturnPool(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // UI쪽 천사 특전 실행 코드
            Debug.Log("천사충돌");
            angelPanel.gameObject.SetActive(true);
        }
    }
}
