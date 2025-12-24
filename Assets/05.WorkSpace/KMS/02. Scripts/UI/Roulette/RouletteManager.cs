using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using STH.Characters.Player;
using STH.ScriptableObjects.Base;

public class RouletteManager : MonoBehaviour
{


    [Header("UI")]
    public GameObject roulettePlate;
    public GameObject roulettePanel;
    public Image[] displayItemSlot;
    public Transform needle;
    public Image resultImage;

    [Header("Skill Data")]
    public SkillData[] skillDatas;
    private SkillData resultSkill;
    List<int> startList = new List<int> ();
    List<int> resultIndexList = new List<int> ();
    int itemCnt = 6;

    [Header("Rotate")]
    public float rotateSpeed;
    private bool isStopping = false;


    private void OnEnable()
    {
        Time.timeScale = 0.0f;


        startList.Clear();
        resultIndexList.Clear();
        isStopping = false;
        rotateSpeed = 10.0f;

        for (int i = 0; i < itemCnt; i++)
        {
            startList.Add (i);
        }

        for (int i = 0; i < itemCnt; i++)
        {
            int randomIndex = Random.Range (0, startList.Count);
            int skillIndex = startList[randomIndex];

            resultIndexList.Add (skillIndex);
            displayItemSlot[i].sprite = skillDatas[skillIndex].icon;
            startList.RemoveAt(randomIndex);
        }

        resultImage.color = new Color(resultImage.color.r, resultImage.color.g, resultImage.color.b, 0.0f);
        StartCoroutine (StartRoulette());
    }

    IEnumerator StartRoulette()
    {
        yield return new WaitForSecondsRealtime(2f);
        
        while (!isStopping)
        {
            roulettePlate.transform.Rotate(0, 0, rotateSpeed);
            yield return null;
        }

        while (rotateSpeed > 0.01f)
        {
            rotateSpeed = Mathf.MoveTowards(rotateSpeed, 0.0f, Time.unscaledDeltaTime * 1.5f);

            
        }

        roulettePlate.transform.Rotate(0, 0, rotateSpeed);
        yield return new WaitForSecondsRealtime(1f);
        Result();
    }

    public void StopRoulette()
    {
        isStopping = true;

    }

    void Result()
    {
        int closetIndex = -1;
        float closetDistance = float.MaxValue;

        for (int i = 0; i < itemCnt; i++)
        {
            float distance = Vector2.Distance(displayItemSlot[i].transform.position, needle.position);
            if (distance < closetDistance)
            {
                closetDistance = distance;
                closetIndex = i;
            }
        }
    

        if (closetIndex == -1)
        {
            Debug.Log("Error");
            return;
        }
        
        int skillIndex = resultIndexList[closetIndex];
        resultSkill = skillDatas[skillIndex];

        resultImage.sprite = resultSkill.icon;
        resultImage.color = new Color(resultImage.color.r, resultImage.color.g, resultImage.color.b, 1.0f);

        ApplyResultSkill();

        Debug.Log (" LV UP " + resultIndexList[closetIndex]);

        StartCoroutine(DisableAfterDelay(2.0f));
    }

    private void ApplyResultSkill()
    {
        if (resultSkill == null) return;

        PlayerController player = FindObjectOfType<PlayerController>();
        if (player == null) return;

        resultSkill.Apply(player);
    }

    IEnumerator DisableAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        this.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }

}
