using UnityEngine;

public class PoolableParticle : MonoBehaviour
{

    private ParticleSystem _ps;

    void Awake()
    {
        _ps = GetComponent<ParticleSystem>();
    }

    void OnEnable()
    {
        // 파티클이 켜지면 재생 시간(Duration) 후에 자동으로 꺼지게 함
        Invoke(nameof(ReturnToPool), _ps.main.duration);
    }

    void ReturnToPool()
    {
        gameObject.SetActive(false);
        GameManager.Pool.ReturnPool(this);
    }

    void OnDisable()
    {
        CancelInvoke(); // 비활성화될 때 혹시 모를 예약 취소
    }
}