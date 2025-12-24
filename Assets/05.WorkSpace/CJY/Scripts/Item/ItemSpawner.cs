using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Coin,
    Equipment,
    Health
}

public class ItemSpawner : MonoBehaviour
{
    [Header("Item Spawn Setting")]
    [SerializeField] private Coin coinPrefab;
    [SerializeField] private Equipment[] equipmentPrefab;
    [SerializeField] private Health healthPrefab;

    int[] dropCoinCount = { 1, 2, 3 };

    //아이템 흡입 시작용 파라미터
    ItemBase[] itembase;

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
        
        var gmInit = GameManager.Pool.transform;
        var parent = gmInit.Find("Item_Pool");
        if(parent == null)
        {
            parent = new GameObject("Item_Pool").transform;
            parent.SetParent(gmInit, false);
        }
        GameManager.Pool.CreatePool(coinPrefab, 30, parent);
        GameManager.Pool.CreatePool(healthPrefab, 10, parent);
        foreach (var equipment in equipmentPrefab) 
        {
            GameManager.Pool.CreatePool(equipment, 5);
        }
    }

    private void AllEnemiesDied(GameObject enemy)
    {
        itembase = FindObjectsOfType<ItemBase>();

        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        EnemyCheck enemycheck = enemy.GetComponent<EnemyCheck>();
        bool isBoss = enemy.TryGetComponent(out Boss boss);
        SpawnItem(enemycheck.endPos, enemyController.Type, isBoss);

        if (IsGetActiveChild())
        {
            CollectAllItems();
        }
    }

    public bool IsGetActiveChild()
    {
        EnemyController[] children = GameManager.Pool.GetComponentsInChildren<EnemyController>(true);

        foreach (EnemyController child in children)
        {
            if (child.gameObject.activeSelf) return false;
        }
        return true;
    }

    public void SpawnItem(Vector3 pos, TypeEnums type, bool isBoss)
    {
        //코인은 항상 드랍, Equipment, Health 는 랜덤드랍 / 보스에서는 필수드랍
        bool randEquipmentDrop = Random.value > 0.9f ? true : false;
        bool randHealthDrop = Random.value > 0.8f ? true : false;

        Coin coin = null;
        Equipment equipment = null;
        Health health = null;

        // enemyType(Melee, Ranged, Boss) 별로 코인갯수 1, 2, 3으로 드롭

        #region 코인,장비,체력 Drop
        if (!isBoss) //보스아님
        {
            switch (type)
            {
                case TypeEnums.Melee:
                    for (int i = 1; i <= dropCoinCount[0]; i++)
                    {
                        coin = GameManager.Pool.GetFromPool(coinPrefab);
                        coin.transform.position = pos;
                    }
                    if(randEquipmentDrop) 
                    {
                        int count = 0;
                        foreach(var equip in equipmentPrefab)
                        {
                            if (count > 0) return;
                            if (Random.value > 0.6f)
                            {
                                equipment = GameManager.Pool.GetFromPool(equip);
                                equipment.transform.position = pos;
                                count++;
                            }
                        }
                    }
                    if (randHealthDrop) 
                    {
                        float count = 0;
                        health = GameManager.Pool.GetFromPool(healthPrefab);
                        health.transform.position = pos + new Vector3(count, 0, count);
                        count += 0.5f;
                    }
                    break;

                case TypeEnums.Ranged:
                    for (int i = 1; i <= dropCoinCount[1]; i++)
                    {
                        coin = GameManager.Pool.GetFromPool(coinPrefab);
                        coin.transform.position = pos + new Vector3(i-1, 0 ,i-1);
                    }
                    if (randEquipmentDrop)
                    {
                        int count = 0;
                        foreach (var equip in equipmentPrefab)
                        {
                            if (count > 0) return;
                            if (Random.value > 0.5f)
                            {
                                equipment = GameManager.Pool.GetFromPool(equip);
                                equipment.transform.position = pos;
                                count++;
                            }
                        }
                    }
                    if (randHealthDrop)
                    {
                        float count = 0;
                        health = GameManager.Pool.GetFromPool(healthPrefab);
                        health.transform.position = pos + new Vector3(count, 0, count);
                        count += 0.5f;
                    }
                    break;
            }
        }
        else //보스임
        {
            for(int i = 1; i <= dropCoinCount[2]; i++)
            {
                coin = GameManager.Pool.GetFromPool(coinPrefab);
                coin.transform.position = pos;
            }
            if (randEquipmentDrop) 
            {
                int count = 0;
                foreach (var equip in equipmentPrefab)
                {
                    if (count > 0) return;
                    if (Random.value > 0.5f)
                    {
                        equipment = GameManager.Pool.GetFromPool(equip);
                        equipment.transform.position = pos;
                        count++;
                    }
                }
            }
            if (randHealthDrop)
            {
                float count = 0;
                health = GameManager.Pool.GetFromPool(healthPrefab);
                health.transform.position = pos + new Vector3(count, 0, count);
                count += 0.5f;
            }
        }
        
    }
    #endregion

    private void CollectAllItems()
    {
        ItemBase[] activeItems = FindObjectsOfType<ItemBase>();

        foreach (var item in activeItems)
        {
            item.Collecting();
        }
    }
}
