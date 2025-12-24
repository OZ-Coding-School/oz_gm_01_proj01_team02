using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageUIManager : MonoBehaviour
{
    public static StageUIManager Instance { get; private set; }

    [SerializeField] public GameObject clearPanel, joyStick;
    [SerializeField] public TextMeshProUGUI chapter, stageNum;

    private void Awake()
    {
        Instance = this;
    }
}
