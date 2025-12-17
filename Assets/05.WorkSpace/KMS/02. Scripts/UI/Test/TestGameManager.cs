using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    public static TestGameManager Instance;

    [Header("HP")]
    public int maxHp = 500;
    public int currentHp;

    [Header("UI")]
    public SegmentedHpBar hpBar;
    public SlotMachineManager slotMachine;


    [Header("HUD")]
    public int exp;
    public int[] nextExp = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
    public int level;
    public int coin;


    public TestPlayer player;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        level = 1;
    }

    private void Start()
    {
        currentHp = maxHp;
        UpdateHpUI();
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
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        UpdateHpUI();
    }

    public void Heal(int amount)
    {
        int newHp = currentHp + amount;
        newHp = Mathf.Clamp(newHp, 0, maxHp);

        currentHp = newHp;
        hpBar.SetHp(currentHp);
    }

    private void UpdateHpUI()
    {
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
}
