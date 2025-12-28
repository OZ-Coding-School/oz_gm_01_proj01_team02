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
    

    

    [Header("UI")]
    
    public SlotMachineManager slotMachine;
    

    [Header("게임씬 전용 데이터")]
    public int exp;
    public int[] nextExp = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
    public int level = 1;
    public int coin { get; private set; }
    

   
    private void Awake()
    {

        Time.timeScale = 1.0f;
        exp = 0;
        coin = 0;
        
    }

    private void Update()
    {
        
    }
 
    public void GetExp(int amount)
    {

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
    }

    
    


}
