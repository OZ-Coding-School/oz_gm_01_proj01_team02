using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 100f;

    void Update()
    {
        // 매 프레임 Z축 방향으로 회전
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
}
