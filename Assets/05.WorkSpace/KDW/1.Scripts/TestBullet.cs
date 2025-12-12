using UnityEngine;

public class TestBullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 10.0f;

    void Start()
    {
        Destroy(gameObject, 3);
    }

    void Update()
    {
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
    }
}
