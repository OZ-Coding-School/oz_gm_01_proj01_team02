using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 옵저버 패턴 사용하여 PoolManager에서 Enemy타입의 ReturnPool이 호출되면 아래의 메서드를 실행
// true가 나왔을 때, portal(EndPoint)을 활성화 시켜주며, Portal의 Open/Close Portal 메서드를 실행

public class EnemyCheck : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public bool IsGetActiveChild()
    {
        Enemy[] children = GameManager.Pool.GetComponentsInChildren<Enemy>();
        foreach (Enemy child in children)
        {
            if (child.gameObject.activeSelf) return false;
        }
        return true;
    }
}
