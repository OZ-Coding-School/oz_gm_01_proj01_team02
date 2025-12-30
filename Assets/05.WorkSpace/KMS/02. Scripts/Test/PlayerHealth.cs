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

    [SerializeField] private DmgText dmgTextPrefab;

    private void Awake()
    {

        if (player == null)
            player = GetComponent<PlayerController>();
        hitEffect = GetComponent<HitEffect>();

        MaxHp = PlayerStatManager.Instance.maxHp;
        currentHp = MaxHp;

        Debug.Log($"[PlayerHealth Awake] InstanceID: {GetInstanceID()}");
    }

    void Start()
    {



    }

    private void Update()
    {
        
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
        Debug.Log($"[PlayerHealth] NotifyHpChanged 호출됨 hp:{currentHp}/{MaxHp}");
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

        if (dmgTextPrefab != null)
        {
            DmgText dmgText = PoolManager.pool_instance.GetFromPool(dmgTextPrefab);
            if (dmgText != null)
            {
                Vector3 worldPos = transform.position + Vector3.up * 2.0f;
                dmgText.gameObject.SetActive(true);
                dmgText.Play(worldPos, amount, isCritical);
            }
        }

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

        GameManager.PlayerisDead();
    }

}
