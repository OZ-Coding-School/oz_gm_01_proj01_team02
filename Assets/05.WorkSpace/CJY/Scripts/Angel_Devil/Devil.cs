using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devil : MonoBehaviour
{
    DevilUIManager devilPanel;
    bool gotIt;
    bool isFirstEnable = true;

    private void OnEnable()
    {
        if (isFirstEnable)
        {
            isFirstEnable = false;
            return;
        }

        SoundManager.Instance.Play("Devil");

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
        // SoundManager.Instance.Stop();

        if (other.CompareTag("Player"))
        {
            if (gotIt) return;
            Debug.Log("�Ǹ� ��ȣ�ۿ�");
            // UI�� �Ǹ� Ư�� ���� �ڵ�
            devilPanel.gameObject.SetActive(true);
            gotIt = true;
        }
    }
}
