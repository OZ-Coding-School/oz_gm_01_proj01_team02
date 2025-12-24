using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearPanel : MonoBehaviour
{
    [SerializeField] private GameObject lootingItemBoxPrefab;
    [SerializeField] private Transform lootingItemZone;
    [SerializeField] private Sprite[] itemSprites;
    int itemCount = 0;

    private void OnEnable()
    {
        SetLootingItem(GameManager.Data.GetItemCount());
    }

    void Update()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            SceneManager.LoadScene("TitleScene");
        }
    }

    public void SetLootingItem(int count)
    {
        if (itemCount == count) return;
        for(int i = 0; i < count; i++)
        {
            GameObject lootingItem = Instantiate(lootingItemBoxPrefab, lootingItemZone, false);
            itemCount++;
        }
    }
    
}
