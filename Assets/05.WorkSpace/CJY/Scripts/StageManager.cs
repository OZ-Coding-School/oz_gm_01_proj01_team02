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

    [Header("Clear Panel Control")]
    [SerializeField] GameObject clearPanel, joyStick;
    [SerializeField] TextMeshProUGUI chapter, stageNum;
    bool onClearPanel = false;

    public static event Action<int> OnStageIncrease;

    public int currentStage { get; private set; } = 1;
    public bool onObstacle { get; private set; } = true;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); 
            return; 
        }

        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            OnClearPanel(1, 1, 1);
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
        string chapter = SceneManager.GetActiveScene().name;
        TestGameManager TGM = FindObjectOfType<TestGameManager>();
        if (currentStage >= CHAPTER_FINISH)
        {
            GameManager.ClearChapter();
            OnClearPanel(TGM.coin, currentStage, int.Parse(chapter[7].ToString()));
        }

        onObstacle = true;
        if (currentStage % STAGE_BRANCH == ANGEL_APPEARS - 1 || currentStage % STAGE_BRANCH == STAGE_BRANCH+ANGEL_APPEARS - 1 || currentStage % STAGE_BRANCH == DEVIL_APPEARS-1 || currentStage % STAGE_BRANCH == STAGE_BRANCH+DEVIL_APPEARS - 1) onObstacle = false;
        currentStage++;
        OnStageIncrease?.Invoke(currentStage);
        //데이터를 저장.
        GameManager.Data.AddData(TGM.coin, currentStage, int.Parse(chapter[7].ToString()));
    }

    public void InitStageClearCount()
    {
        currentStage = 1;
    }

    public void OnClearPanel(int stage, int chapter, int coin)
    {

        onClearPanel = !onClearPanel;
        clearPanel.SetActive(onClearPanel);
        joyStick.SetActive(!onClearPanel);


    }

}
