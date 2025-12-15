using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachineManager : MonoBehaviour
{
    public GameObject[] slotSkillObject;
    public Button[] slot;
    public Sprite[] skillSprite;
    [System.Serializable]
    public class DisplayItemSlot
    {
        public List<Image> slotSprite = new List<Image> ();
    }
    public DisplayItemSlot[] displayItemSlots;
    public Image displayResultImage;

    public List<int> startList = new List<int> ();
    public List<int> resultIndexList = new List<int> ();
    int itemCnt = 3;


    void Start()
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
                if ( i == 0 && j == 1 || i == 1 && j == 0 || i == 2 && j == 2)
                {
                    resultIndexList.Add (startList [randomIndex]);
                }
                displayItemSlots[i].slotSprite[j].sprite = skillSprite[startList[randomIndex]];

                if ( j == 0)
                {
                    displayItemSlots[i].slotSprite[itemCnt].sprite = skillSprite[startList[randomIndex]];
                }
                startList.RemoveAt (randomIndex);
            }
        }

        StartCoroutine ( StartSlot1 ());
        StartCoroutine ( StartSlot2 ());
        StartCoroutine ( StartSlot3 ());
    }
    

    IEnumerator StartSlot1()
    {

        yield return null;
        for (int i = 0; i < (itemCnt*10 + 5)*2; i++)
        {
            slotSkillObject[0].transform.localPosition -= new Vector3 ( 0 , 50f, 0);
            if (slotSkillObject[0].transform.localPosition.y < 50f)
            {
                slotSkillObject[0].transform.localPosition += new Vector3 (0, 150f, 0);
            }
            yield return new WaitForSeconds (0.02f);
        }
        for ( int i = 0; i < itemCnt; i++)
        {
            slot[i].interactable = true;
        }
    }

    IEnumerator StartSlot2()
    {
        for ( int i = 0; i < (itemCnt * 10 + 2) * 2; i ++)
        {
            slotSkillObject[1].transform.localPosition -= new Vector3 (0, 50f, 0);
            if (slotSkillObject[1].transform.localPosition.y < 50f)
            {
                slotSkillObject[1].transform.localPosition += new Vector3 ( 0, 150f, 0);
            }
            yield return new WaitForSeconds (0.02f);
        }
        for (int i = 0; i < itemCnt; i++)
        {
            slot[i].interactable = true;
        }
    }

    IEnumerator StartSlot3()
    {
        for ( int i = 0; i < (itemCnt * 14 + 2) *2 ; i ++)
        {
            slotSkillObject[2].transform.localPosition -= new Vector3 (0, 50f, 0);
            if (slotSkillObject[2].transform.localPosition.y < 50f)
            {
                slotSkillObject[2].transform.localPosition += new Vector3 ( 0, 150f, 0);
            }
            yield return new WaitForSeconds (0.02f);
        }
        for (int i = 0; i < itemCnt; i++)
        {
            slot[i].interactable = true;
        }
    }



    
}
