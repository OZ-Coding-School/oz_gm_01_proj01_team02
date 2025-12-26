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

    [Header("HP")]
    public int maxHp = 500;
    public int currentHp;

    [Header("UI")]
    public SegmentedHpBar hpBar;
    public SlotMachineManager slotMachine;
    public GameObject pauseUI;

    [Header("HUD")]
    public int exp;
    public int[] nextExp = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
    public int level = 1;
    public int coin { get; private set; }

    [Header("State")]
    public TestGameState gameState = TestGameState.Playing;


    public PlayerMove player;
    //public TestPlayer player;
    private void OnEnable()
    {
        //player = FindObjectOfType<PlayerController>(); 
        mapPanel = FindObjectOfType<MapPanel>(); 
        hpBar = FindObjectOfType<SegmentedHpBar>();
        Canvas canvas = FindObjectOfType<Canvas>();
        slotMachine = canvas.transform.Find("SlotMachineManager/SlotMachineUI")?.GetComponent<SlotMachineManager>();
        pauseUI = canvas.transform.Find("PauseUI")?.gameObject;
    }
    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            // SceneManager.sceneLoaded += OnSceneLoaded;
        }
                
        else Destroy(gameObject);

        //player = FindObjectOfType<PlayerController>(); 
        mapPanel = FindObjectOfType<MapPanel>(); 
        hpBar = FindObjectOfType<SegmentedHpBar>();
        Canvas canvas = FindObjectOfType<Canvas>();
        slotMachine = canvas.transform.Find("SlotMachineManager/SlotMachineUI")?.GetComponent<SlotMachineManager>();
        pauseUI = canvas.transform.Find("PauseUI")?.gameObject;

        Time.timeScale = 1.0f;
        gameState = TestGameState.Playing;
    }

    // private void OnSceneLoaded(Scene scene, LoadS)

    private void Start()
    {
        currentHp = maxHp;
        UpdateHpUI();
        gameState = TestGameState.Playing;

    }

    public void SetSelectedMap(MapData data)
    {
        SelectedMap = data;

    }    


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TakeDamage(50);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(30);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1.0f;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        UpdateHpUI();
    }

    public void Heal(int amount)
    {
        if (hpBar == null)
        {
            return;
        };
        int newHp = currentHp + amount;
        newHp = Mathf.Clamp(newHp, 0, maxHp);

        currentHp = newHp;
        hpBar.SetHp(currentHp);
    }

    private void UpdateHpUI()
    {
        if (hpBar == null)
        {
            return;
        };
        hpBar.SetHp(currentHp);
    }


    public void GetExp(int amount)
    {

        exp+=amount;

        if ( exp == nextExp[Mathf.Min(level, nextExp.Length-1 )])
        {
            level++;
            exp = 0;

            slotMachine.PlaySlotMachine();
        }
    }

    public void GetCoin(int amount)
    {
        coin += amount;
    }

    public void TogglePause()
    {
        if (gameState == TestGameState.Paused)
            Resume();
        else
            Pause();

    }

    public void Pause()
    {
        
        gameState = TestGameState.Paused;
        Time.timeScale = 0.0f;
        pauseUI.SetActive(true);

    }

    public void Resume()
    {
        gameState = TestGameState.Playing;
        Time.timeScale = 1.0f;
        pauseUI.SetActive(false);
    }

    public void GoHome()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("TitleScene(Build)");
        
    }

    public void DecreaseMaxHp(int amount)
    {
        maxHp -= amount;
        Debug.Log($"{maxHp}");
        maxHp = Mathf.Max(1, maxHp);
        currentHp = Mathf.Min(currentHp, maxHp);
        UpdateHpUI();
    }

    // public void AddBuff(AngelBuffData buff)
    // {
    //     Debug.Log($"악마 계약: {buff.displayName}");
    // }

}
