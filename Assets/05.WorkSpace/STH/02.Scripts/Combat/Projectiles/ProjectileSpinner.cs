using UnityEngine;

public class ProjectileSpinner : MonoBehaviour
{
    [Header("회전 속도")]
    [SerializeField] private float spinSpeed = 720f; // 초당 회전 각도

    [Header("회전축 설정")]
    [SerializeField] private Vector3 rotationAxis = Vector3.up; // 보통 Y축(up)이나 Z축(forward)

    void Update()
    {
        // 매 프레임 지정된 축을 중심으로 회전
        transform.Rotate(rotationAxis * spinSpeed * Time.deltaTime);
    }
}