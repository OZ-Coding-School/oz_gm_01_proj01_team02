using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Portal : MonoBehaviour
{
    MeshRenderer[] mr;
   
    [SerializeField] GameObject endPoint;
    [SerializeField] Material[] materials;


    private void OnEnable()
    {
        EnemyCheck.OnEnemyReturnPool += AllEnemiesDied;
    }

    private void OnDisable()
    {
        EnemyCheck.OnEnemyReturnPool -= AllEnemiesDied;
    }

    private void Start()
    {
        mr = GetComponentsInChildren<MeshRenderer>(true);
        endPoint.SetActive(false);
    }

    private void AllEnemiesDied(GameObject enemy)
    {
        if (IsGetActiveChild()) OpenPortal();
        else ClosePortal();
    }

    public bool IsGetActiveChild()
    {
        Debug.Log("남은 적 검사");
        EnemyController[] children = GameManager.Pool.GetComponentsInChildren<EnemyController>(true);
        
        foreach (EnemyController child in children)
        {
            if (child.gameObject.activeSelf) return false;
        }
        return true;
    }

    public void OpenPortal()
    {
        endPoint.SetActive(true);
        foreach(var m in mr)
        {
            Material[] mats = m.materials;
            if (mats.Length > 0) mats[0] = materials[1];
            m.materials = mats;
        }
    }

    public void ClosePortal()
    {
        endPoint.SetActive(false);
        foreach (var m in mr)
        {
            Material[] mats = m.materials;
            if (mats.Length > 0) mats[0] = materials[0];
            m.materials = mats;
        }
    }
}
