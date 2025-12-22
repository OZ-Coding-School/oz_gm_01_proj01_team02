using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public const int DEVIL_APPEARS = 5;
    public const int ANGEL_APPEARS = 3;
    public const int STAGE_BRANCH = 5;
    public const int CHAPTER_FINISH = 15;

    public static event Action<int> OnStageIncrease;

    public int currentStage { get; private set; } = 1;
    public bool onObstacle { get; private set; } = true;

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
        OnStageIncrease?.Invoke(currentStage);
    }

    public void InitStageClearCount()
    {
        currentStage = 1;
    }

   



    // 천사 : 특정 스테이지 뒷글자가 5인 스테이지마다 등장
    // 악마 : 보스전 이후 등장
    // 평상시에는 일반적, 장애물 생성
    // 뒷자리 5인 스테이지마다 일반적, 장애물 생성 x 천사 생성
    // 10스테이지 마다 일반적, 장애물 생성 x 보스전 종료 후 악마 생성

    //IEnumerator SpawnStage()
    //{

    //}

}
