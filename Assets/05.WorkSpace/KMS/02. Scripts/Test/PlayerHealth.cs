using System.Collections;
using System.Collections.Generic;
using STH.Characters.Player;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    
    [Header("Reference")]
    private SegmentedHpBar hpBar;
    [SerializeField] private PlayerController player;
    
    public float CurrentHp => currentHp;
    public float MaxHp => PlayerStatManager.Instance.maxHp;
    [SerializeField] private float currentHp;

    public event System.Action<float, float> OnHpChanged;


    private void Awake()
    {
        
        if (player == null)
        player = GetComponent<PlayerController>();
        
    }

    private void Start()
    {
        currentHp = MaxHp;
        NotifyHpChanged();
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

    private void OnEnable()
    {
        PlayerStatManager.OnStatChanged += ChangeHp;
    }

    private void OnDisable()
    {
        PlayerStatManager.OnStatChanged -= ChangeHp;
    }

    private void NotifyHpChanged()
    {
        OnHpChanged?.Invoke(currentHp, MaxHp);
    }

    public void TakeDamage(float amount)
    {

        currentHp -= amount;
        currentHp = Mathf.Clamp(currentHp, 0, MaxHp);
        NotifyHpChanged();
    
    }

    public void Heal(float amount)
    {
        currentHp += amount;
        currentHp = Mathf.Clamp(currentHp, 0, MaxHp);
        NotifyHpChanged();
    }


    private void ChangeHp()
    {
        currentHp = Mathf.Min(currentHp, MaxHp);
        NotifyHpChanged();
    }

   


}
