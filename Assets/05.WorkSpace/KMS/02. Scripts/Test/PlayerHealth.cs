using System.Collections;
using System.Collections.Generic;
using STH.Characters.Player;
using UnityEngine;
using STH.Core;

public class PlayerHealth : MonoBehaviour, IDamageable
{

    [Header("Reference")]
    private SegmentedHpBar hpBar;
    [SerializeField] private PlayerController player;

    [SerializeField] private float currentHp;
    public float MaxHp;
    public float CurrentHp => currentHp;

    public event System.Action<float, float> OnHpChanged;

    private HitEffect hitEffect;

    private bool canTakeDamage = true;

    private void Awake()
    {

        if (player == null)
            player = GetComponent<PlayerController>();
        hitEffect = GetComponent<HitEffect>();


    }

    void Start()
    {
        MaxHp = PlayerStatManager.Instance.maxHp;
        currentHp = MaxHp;
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Q))
        // {
        //     TakeDamage(50);
        // }

        // if (Input.GetKeyDown(KeyCode.H))
        // {
        //     Heal(30);
        // }

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

    private IEnumerator ResetSuperTime()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(player.Stats.superTime);
        canTakeDamage = true;
    }

    public void TakeDamage(float amount, bool isCritical = false)
    {
        if (player.IsDead || !canTakeDamage) return;

        StartCoroutine(ResetSuperTime());

        if (hitEffect != null) hitEffect.PlayHitEffect(isCritical);
        Debug.Log($"Player Take Damage: {amount}, Critical: {isCritical}");

        currentHp -= amount;
        currentHp = Mathf.Clamp(currentHp, 0, MaxHp);
        NotifyHpChanged();

        if (currentHp <= 0)
        {
            Die();
        }
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

    public void Die()
    {
        player.Animator.SetTrigger("Die");
        player.IsDead = true;
    }

}
