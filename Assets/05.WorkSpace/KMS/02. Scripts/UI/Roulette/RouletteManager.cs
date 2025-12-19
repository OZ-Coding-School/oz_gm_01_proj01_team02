using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteManager : MonoBehaviour
{
    public GameObject roulettePlate;
    public GameObject roulettePanel;
    public Transform needle;
    public Image resultImage;

    public Sprite[] skillSprite;
    public Image[] displayItemSlot;

    List<int> startList = new List<int> ();
    List<int> resultIndexList = new List<int> ();
    int itemCnt = 6;

    
    

    private bool isStopping = false;
    public float rotateSpeed = 10.0f;

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

        resultImage.sprite = displayItemSlot[6].sprite;
        resultImage.color = new Color(resultImage.color.r, resultImage.color.g, resultImage.color.b, 0.0f);
        StartCoroutine (StartRoulette());

    }

    private void OnEnable()
    {
        Time.timeScale = 0.0f;
    }

    IEnumerator StartRoulette()
    {
        yield return new WaitForSeconds(2f);
        
        while (true)
        {
            yield return null;
            
            if (isStopping)
            {
                rotateSpeed = Mathf.Lerp(rotateSpeed, 0, Time.deltaTime*2f);
            }
            
            if (rotateSpeed <= 0.01f) break;

            roulettePlate.transform.Rotate(0, 0, rotateSpeed);

            if (rotateSpeed <= 0.01f && isStopping) break;
        }

        yield return new WaitForSeconds(1f);
        Result();
    }

    public void StopRoulette()
    {
        isStopping = true;
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
        resultImage.color = new Color(resultImage.color.r, resultImage.color.g, resultImage.color.b, 1.0f);

        Debug.Log (" LV UP " + resultIndexList[closetIndex]);

        StartCoroutine(DisableAfterDelay(2.0f));
    }

    IEnumerator DisableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        this.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }

}
