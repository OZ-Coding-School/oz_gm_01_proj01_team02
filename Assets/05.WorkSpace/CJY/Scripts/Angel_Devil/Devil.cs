using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devil : MonoBehaviour
{
    DevilUIManager devilPanel;
    bool gotIt;

    private void OnEnable()
    {
        devilPanel = FindObjectOfType<DevilUIManager>(true);
        gotIt = false;
    }
    public void ReturnPool()
    {
        gotIt = false;
        if (PoolManager.pool_instance != null) PoolManager.pool_instance.ReturnPool(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gotIt) return;
            Debug.Log("악마 상호작용");
            // UI쪽 악마 특전 실행 코드
            devilPanel.gameObject.SetActive(true);
            gotIt = true;
        }
    }
}
