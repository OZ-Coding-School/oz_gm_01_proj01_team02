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

    

    [Header("UI")]
    public SegmentedHpBar hpBar;
    public SlotMachineManager slotMachine;
    public GameObject pauseUI;
    [SerializeField] Canvas canvas;

    [Header("HUD")]
    public int exp;
    public int[] nextExp = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
    public int level = 1;
    public int coin { get; private set; }
    



    // public PlayerMove player; -> 이게 원래 코드임
    //public TestPlayer player;
    private void OnEnable()
    {
        player = FindObjectOfType<PlayerController>(); 
        mapPanel = FindObjectOfType<MapPanel>(); 
        hpBar = FindObjectOfType<SegmentedHpBar>();
    }
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
            //// SceneManager.sceneLoaded += OnSceneLoaded;
        }

        //else Destroy(gameObject);

        player = FindObjectOfType<PlayerController>(); 
        mapPanel = FindObjectOfType<MapPanel>(); 
        hpBar = FindObjectOfType<SegmentedHpBar>();
        

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
            slotMachine.gameObject.SetActive(true);
            slotMachine.PlaySlotMachine();
        }
    }

    public void GetCoin(int amount)
    {
        coin += amount;
    }

    
    


}
