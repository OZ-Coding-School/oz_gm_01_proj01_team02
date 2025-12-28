using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    public const int DEVIL_APPEARS = 5;
    public const int ANGEL_APPEARS = 3;
    public const int STAGE_BRANCH = 5;
    public const int CHAPTER_FINISH = 15;

    TestGameManager TGM;
    public string chapter;

   [Header("Clear Panel Control")]
    GameObject clearPanel, joyStick;
    TextMeshProUGUI chapterTxt, stageNumTxt;
    bool onClearPanel = false;

    public static event Action<int> OnStageIncrease;

    public int currentStage { get; private set; } = 1;
    public bool onObstacle { get; private set; } = true;

    private void OnEnable()
    {

        if (instance == null) instance = this;
        //else
        //{
        //    Destroy(gameObject);
        //    return;

        //}
        ////if (instance != null && instance != this)
        ////{
        ////    Destroy(gameObject); 
        ////    return; 
        ////}

        //instance = this;

        if (StageUIManager.Instance != null) 
        {
            this.clearPanel = StageUIManager.Instance.clearPanel;
            this.joyStick = StageUIManager.Instance.joyStick;
            this.chapterTxt = StageUIManager.Instance.chapter;
            this.stageNumTxt = StageUIManager.Instance.stageNum;
        }

        chapter = SceneManager.GetActiveScene().name;
        TGM = FindObjectOfType<TestGameManager>();
        Debug.Log("현재 스테이지 : " + currentStage);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
           GameManager.GameOver();
        }
    }

    public int Select(string name)
    {
        if(name == "angel") return ANGEL_APPEARS;
        if (name == "devil") return DEVIL_APPEARS;
        if (name == "stage") return STAGE_BRANCH;
        if (name == "finish") return CHAPTER_FINISH;
        else return 0;
    }

    public void StageIncrease()
    {
       
        onObstacle = true;
        if (currentStage % STAGE_BRANCH == ANGEL_APPEARS - 1 || currentStage % STAGE_BRANCH == STAGE_BRANCH+ANGEL_APPEARS - 1 || currentStage % STAGE_BRANCH == DEVIL_APPEARS-1 || currentStage % STAGE_BRANCH == STAGE_BRANCH+DEVIL_APPEARS - 1) onObstacle = false;
        currentStage++;
        Debug.Log("현재 스테이지 : " + currentStage);
        OnStageIncrease?.Invoke(currentStage);
        //데이터를 저장.
        GameManager.Data.AddData(TGM.coin, TGM.exp, currentStage, int.Parse(chapter[7].ToString()));
    }

    public void InitStageClearCount()
    {
        currentStage = 1;
    }

    public void OnClearPanel(int coin, int exp, int stage, int chapter)
    {
        chapterTxt.text = "Chapter "+ chapter.ToString();
        stageNumTxt.text = stage.ToString();
        Debug.Log(clearPanel is null);

        onClearPanel = !onClearPanel;
        clearPanel.SetActive(onClearPanel);
        joyStick.SetActive(!onClearPanel);
    }

}
