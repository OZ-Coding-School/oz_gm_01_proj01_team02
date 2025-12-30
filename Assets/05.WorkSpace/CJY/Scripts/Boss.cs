using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    private void OnDisable()
    {
        if (GameManager.Stage.currentStage == GameManager.Stage.Select("finish"))
        {
            StartCoroutine(DelayDie());
        }
    }

    IEnumerator DelayDie()
    {
        yield return new WaitForSeconds(0.5f);
        GameManager.GameOver();
    }
}

