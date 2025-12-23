using UnityEngine;

public class ColliderFitter : MonoBehaviour
{
    // 이 함수를 인스펙터 우클릭 메뉴에서 실행할 수 있게 합니다.
    [ContextMenu("Fit Collider to Children")]
    public void FitColliderToChildren()
    {
        // 1. 자식들의 모든 렌더러를 찾아 전체 영역(Bounds) 계산
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return;

        Bounds bounds = renderers[0].bounds;
        foreach (Renderer ren in renderers)
        {
            bounds.Encapsulate(ren.bounds);
        }

        // 월드 좌표의 중심점을 로컬 좌표로 변환
        Vector3 localCenter = transform.InverseTransformPoint(bounds.center);
        Vector3 localSize = transform.InverseTransformVector(bounds.size);
        localSize = new Vector3(Mathf.Abs(localSize.x), Mathf.Abs(localSize.y), Mathf.Abs(localSize.z));

        // 2. 콜라이더 종류별 대응

        // Sphere Collider인 경우
        if (TryGetComponent(out SphereCollider sphere))
        {
            sphere.center = localCenter;
            // 가로, 세로, 높이 중 가장 큰 값을 기준으로 반지름 설정
            sphere.radius = Mathf.Max(localSize.x, localSize.y, localSize.z) * 0.5f;
        }
        // Box Collider인 경우
        else if (TryGetComponent(out BoxCollider box))
        {
            box.center = localCenter;
            box.size = localSize;
        }
        // Capsule Collider인 경우
        else if (TryGetComponent(out CapsuleCollider capsule))
        {
            capsule.center = localCenter;
            // 축에 따라 다르지만 보통 Y축 기준: 가로/깊이 중 큰 값이 반지름, 세로가 높이
            capsule.radius = Mathf.Max(localSize.x, localSize.z) * 0.5f;
            capsule.height = localSize.y;
        }

        Debug.Log($"{gameObject.name}의 콜라이더가 자식 메쉬에 맞춰 최적화되었습니다.");
    }
}