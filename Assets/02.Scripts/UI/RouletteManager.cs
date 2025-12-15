using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteManager : MonoBehaviour
{
    public GameObject roulettePlate;
    public GameObject roulettePanel;
    public Transform needle;

    public Sprite[] skillSprite;
    public Image[] displayItemSlot;

    List<int> startList = new List<int> ();
    List<int> resultIndexList = new List<int> ();
    int itemCnt = 6;

    void Start()
    {
        for (int i = 0; i < itemCnt; i++)
        {
            startList.Add (i);
        }

        for (int i = 0; i < itemCnt; i++)
        {
            int randomIndex = Random.Range (0, startList.Count);
            resultIndexList.Add (startList[randomIndex]);
            displayItemSlot[i].sprite = skillSprite[startList[randomIndex]];
            startList.RemoveAt(randomIndex);
        }

        StartCoroutine (StartRoulette());

    }

    IEnumerator StartRoulette()
    {
        yield return new WaitForSeconds(2f);
        float rotateSpeed = 100.0f;
        while (true)
        {
            yield return null;
            if (rotateSpeed <= 0.01f) break;

            rotateSpeed = Mathf.Lerp (rotateSpeed, 0, Time.deltaTime * 2f);
            roulettePlate.transform.Rotate(0, 0, rotateSpeed);
        }

        yield return new WaitForSeconds(1f);
        Result();
    }

    void Result()
    {
        int closetIndex = -1;
        float closeDis = 500f;
        float currentDis = 0f;

        for (int i = 0; i < itemCnt; i++)
        {
            currentDis = Vector2.Distance(displayItemSlot[i].transform.position, needle.position);
            if (closeDis > currentDis)
            {
                closeDis = currentDis;
                closetIndex = i;
            }
        }
    

        if (closetIndex == -1)
        {
            Debug.Log("Error");
        }
        displayItemSlot[itemCnt].sprite = displayItemSlot[closetIndex].sprite;

        Debug.Log (" LV UP " + resultIndexList[closetIndex]);
    }

}
