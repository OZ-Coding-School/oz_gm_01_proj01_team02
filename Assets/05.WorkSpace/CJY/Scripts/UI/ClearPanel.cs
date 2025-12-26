using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearPanel : MonoBehaviour
{
    [SerializeField] private GameObject lootingItemBoxPrefab;
    [SerializeField] private Transform lootingItemZone;

    private Dictionary<string, Sprite> itemSprites = new Dictionary<string, Sprite>();
    private List<GameObject> lootingItemBoxes = new List<GameObject>();

    int itemCount = 0;
    [SerializeField] TestGameManager TGM;

    bool canSkip = false;

    private void Awake()
    {
        LoadItemSprites();
    }

    private void OnEnable()
    {
        Time.timeScale = 0f;
        canSkip = false;
        StartCoroutine(DelayTabToClose());
        foreach (GameObject obj in lootingItemBoxes)
        {
            if (obj != null) Destroy(obj);
        }
        lootingItemBoxes.Clear();

        if (GameManager.Data == null || GameManager.Data.collectedItemName == null) return;

        List<string> nameList = GameManager.Data.collectedItemName;
        Dictionary<string, int> countDict = GameManager.Data.collectedItem;

        for (int i = 0; i < nameList.Count; i++)
        {
            string itemName = nameList[i];

            if (countDict.ContainsKey(itemName))
            {
                int count = countDict[itemName];
                CreateLootingBox(itemName, count);
            }

        }
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    void Update()
    {
        if(!canSkip) return;
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            SceneManager.LoadScene("TitleScene(Build)");
            
        }
    }

    private void CreateLootingBox(string name, int count)
    {
        GameObject go = Instantiate(lootingItemBoxPrefab, lootingItemZone, false);

        lootingItemBoxes.Add(go);

        SetLootingBox goSLB = go.GetComponent<SetLootingBox>();
        if(goSLB != null)
        {
            Sprite sprite = GetSpriteByName(name);
            goSLB.SetData(sprite, count);
        }
    }

    private void LoadItemSprites()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Items");

        foreach (Sprite sp in sprites)
        {
            if (!itemSprites.ContainsKey(sp.name))
            {
                itemSprites.Add(sp.name, sp);
            }
        }
    }

    public Sprite GetSpriteByName(string name)
    {
        string delClone = name.Replace("(Clone)", "").Trim().ToLower();

        if(itemSprites.ContainsKey(delClone)) return itemSprites[delClone];

        return null;
    }
    
    IEnumerator DelayTabToClose()
    {
        yield return new WaitForSecondsRealtime(2);
        canSkip = true;
    }
}
