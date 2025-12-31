using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angel : MonoBehaviour
{
    AngelUIManagers angelPanel;
    bool gotIt;
    bool isFirstEnable = true;

    private void OnEnable()
    {
        if (isFirstEnable)
        {
            isFirstEnable = false;
            return;
        }

        SoundManager.Instance.Play("Angel");

        angelPanel = FindObjectOfType<AngelUIManagers>(true);
        gotIt = false;
    }

    public void ReturnPool()
    {
        gotIt = false;
        if (PoolManager.pool_instance != null) PoolManager.pool_instance.ReturnPool(this);
    }

    private void OnTriggerEnter(Collider other)
    {

        // SoundManager.Instance.Stop();

        if (other.CompareTag("Player"))
        {
            if (gotIt) return;
            // UI�� õ�� Ư�� ���� �ڵ�
            Debug.Log("õ���浹");
            angelPanel.gameObject.SetActive(true);
            gotIt = true;
        }
    }
}
