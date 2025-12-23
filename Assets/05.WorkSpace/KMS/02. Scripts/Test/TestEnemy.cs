using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float maxHp = 1000f;  // 테스트용으로 체력 높게 설정
    public float currentHp;

    private void Awake()
    {
        currentHp = maxHp;
    }

    // 데미지 받기
    public void TakeDamage(float amount)
    {
        currentHp -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. Current HP: {currentHp}");

        if (currentHp <= 0)
        {
            Die();
        }
    }

    // 죽음 처리
    private void Die()
    {
        Debug.Log($"{gameObject.name} died.");
        gameObject.SetActive(false); // 풀링용 비활성화
    }
}
