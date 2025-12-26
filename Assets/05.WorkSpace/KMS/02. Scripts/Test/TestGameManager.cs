using STH.Characters.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestGameManager : MonoBehaviour
{
    public static TestGameManager Instance;
    private MapPanel mapPanel;
    public MapData SelectedMap;
    public PlayerController player;

    


    [Header("HUD")]
    public int exp;
    public int[] nextExp = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
    public int level = 1;
    public int coin { get; private set; }
    



    
    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
                
        else Destroy(gameObject);
        

        Time.timeScale = 1.0f;
        
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = FindObjectOfType<PlayerController>(); 
        
    }

    private void Start()
    {


    }

    public void SetSelectedMap(MapData data)
    {
        SelectedMap = data;
    }    


    private void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1.0f;
        }
    }
    
    public void GetExp(int amount)
    {

        exp+=amount;

        if ( exp == nextExp[Mathf.Min(level, nextExp.Length-1 )])
        {
            level++;
            exp = 0;

            UIManager.Instance.slotMachine.PlaySlotMachine();
        }
    }

    public void GetCoin(int amount)
    {
        coin += amount;
    }

    
    


}
