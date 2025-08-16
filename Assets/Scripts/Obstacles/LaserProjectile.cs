using UnityEngine;
public class LaserProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float speed = 15f; // 레이저 속도
    [SerializeField] private float lifeTime = 2f; // 레이저 지속시간
    [SerializeField] private float damage = 10f; // 레이저 대미지

    private Vector3 direction; // 레이저 방향

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
    public void Init(Vector3 dir) // 레이저 프리팹 초기화
    {
        direction = dir.normalized;
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 데미지 관련 내용 추가
        Destroy(gameObject);
    }

}
