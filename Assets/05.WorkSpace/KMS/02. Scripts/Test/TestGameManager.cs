using STH.Characters.Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestGameManager : MonoBehaviour
{
    public static TestGameManager Instance;

    [Header("Player Data (ScriptableObject)")]
    public PlayerData playerData;
    

    [Header("UI")]
    public SlotMachineManager slotMachine;
    

    [Header("게임씬 전용 데이터")]
    public int exp;
    public int[] nextExp = { 100, 100, 100, 150, 150, 150, 150, 200, 200};
    public int level = 1;
    public int maxLevel = 10;
    public int coin { get; private set; }
    [SerializeField] private DmgText dmgTextPrefab;
    

    private void Awake()
    {
        if(Instance == null) Instance = this;
        


        Time.timeScale = 1.0f;
        exp = 0;
        coin = 0;
        
    }

    private void Start()
    {
        PoolManager.pool_instance.CreatePool(dmgTextPrefab, 30);
    }

 
    public void GetExp(int amount)
    {
        if (level >= maxLevel)
        return;

        Debug.Log("GETEXP 실행");
        exp+=amount;

        if ( exp == nextExp[Mathf.Min(level, nextExp.Length-1 )])
        {
            level++;
            exp = 0;
            slotMachine.gameObject.SetActive(true);
            slotMachine.PlaySlotMachine();
        }
    }

    public void GetCoin(int amount)
    {
        coin += amount;

        if (playerData != null)
        {
            playerData.AddCoin(amount);
        }

        int adventureExp = Mathf.FloorToInt(amount * 0.1f);
        if (adventureExp > 0)
        {
            playerData.AddadventrueExp(adventureExp);
        }
    }

    public void InitCoinExp()
    {
        coin = 0;
        exp = 0;
    }

    
    
    

}
