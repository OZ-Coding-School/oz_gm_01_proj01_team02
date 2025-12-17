using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachineManager : MonoBehaviour
{
    public GameObject slotMachineUI;
    public GameObject[] slotSkillObject;
    public Button[] slot;
    public Sprite[] skillSprite;
    public GameObject hpBar;
    [System.Serializable]
    public class DisplayItemSlot
    {
        public List<Image> slotSprite = new List<Image> ();
    }
    public DisplayItemSlot[] displayItemSlots;
    public Image displayResultImage;

    public List<int> startList = new List<int> ();
    public List<Sprite> resultSpriteList = new List<Sprite>();
    int itemCnt = 3;


    void OnEnable()
    {
        displayResultImage.sprite = null;
        
    }

    int FinalSprite(int index)
    {
        int totalImages = displayItemSlots[index].slotSprite.Count;
        float yPos = slotSkillObject[index].transform.localPosition.y;
        float imageHeight = displayItemSlots[index].slotSprite[0].rectTransform.rect.height;
        int centralIndex = Mathf.RoundToInt(yPos/ imageHeight);

        centralIndex = Mathf.Clamp(centralIndex, 0, totalImages - 1);

        return centralIndex;

    }
    
    public void PlaySlotMachine()
    {
        for ( int i = 0; i < itemCnt * slot.Length; i++)
        {
            startList.Add(i);
        }

        for ( int i = 0; i < slot.Length; i++)
        {
            for (int j = 0; j < itemCnt; j++)
            {
                slot[i].interactable = false;

                int randomIndex = Random.Range (0, startList.Count);
                
                displayItemSlots[i].slotSprite[j].sprite = skillSprite[startList[randomIndex]];

                if ( j == 0)
                {
                    displayItemSlots[i].slotSprite[itemCnt].sprite = skillSprite[startList[randomIndex]];
                }
                startList.RemoveAt (randomIndex);
            }
        }
        Show();
        StartCoroutine ( StartSlot(0, (itemCnt*10 +5)*2));
        StartCoroutine ( StartSlot(1, (itemCnt*10 + 2)*2));
        StartCoroutine ( StartSlot(2, (itemCnt*14 + 2)*2));
    }

    IEnumerator StartSlot(int index, int repeatCount)
    {

        yield return null;
        for (int i = 0; i < (itemCnt*10 + 5)*2; i++)
        {
            slotSkillObject[index].transform.localPosition -= new Vector3 ( 0 , 50f, 0);
            if (slotSkillObject[index].transform.localPosition.y < 50f)
            {
                slotSkillObject[index].transform.localPosition += new Vector3 (0, 150f, 0);
            }
            yield return new WaitForSeconds (0.02f);
        }

        int finalIndex = FinalSprite(index);
        Sprite finalSprite = displayItemSlots[index].slotSprite[finalIndex].sprite;

        if (resultSpriteList.Count <= index)
            resultSpriteList.Add(finalSprite);
        else
            resultSpriteList[index] = finalSprite;

        
        
        slot[index].interactable = true;
    }

    public void ClickBtn(int index)
    {
        displayResultImage.sprite = resultSpriteList[index];
        StartCoroutine(DisableAfterDelay(1f));
        
    }
    
    private IEnumerator DisableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        slotMachineUI.SetActive(false);
        hpBar.SetActive(true);
    }

    public void Show()
    {
        hpBar.SetActive(false);
        slotMachineUI.SetActive(true);
        displayResultImage.sprite = null;
    }



    
}
