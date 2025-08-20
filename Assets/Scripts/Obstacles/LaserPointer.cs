using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : ObstacleBase
{
    [SerializeField] private Transform startPoint; // 포인터 시작점
    [SerializeField] Transform target; // 타겟
    [SerializeField] LayerMask obstacleMask;  // 타겟 이외 레이어마스크(벽, 장애물 등)
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float laserLength = 10f; // 레이저 길이
    private bool isSensorMode = false; // 감시 모드

    private Ray ray;             
    private RaycastHit rayHit;  

    void Start()
    {
        if(lineRenderer == null)
        {  
            lineRenderer = GetComponent<LineRenderer>(); 
        }

        lineRenderer.positionCount = 2; // 레이저 포인터 점 갯수
    }

    void Update()
    {
        if (isSensorMode)
        {
            SensorMode();
        }
        else
        {
            TurretMode();
        }
    }

    // 콜백으로 ray에 뭔가 맞았을때는 onHit, 맞지 않았을때는 onMiss 콜백 실행
    private void ShootLaser(Vector3 direction, float distance, System.Action<RaycastHit> onHit = null, System.Action onMiss = null)
    {
        ray = new Ray(startPoint.position, direction);
        Vector3 _endPoint = startPoint.position + direction * distance;

        if(Physics.Raycast(ray, out rayHit, distance, obstacleMask))
        {
            _endPoint = rayHit.point;
            onHit?.Invoke(rayHit);
        }
        else
        {
            onMiss?.Invoke();
        }

        OnLaser(startPoint.position, _endPoint);
    }

    public void SensorMode()
    {
        // 감지 장치 모드
        Vector3 _direction = startPoint.forward; // 정면으로 레이저 발사
        ShootLaser(_direction, laserLength, onHit: (hit) =>
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Debug.Log("플레이어 감지");
                // 감지 후 이벤트 추가 (ex 경고 발생, 터렛 작동, 사망 등등)
            }
        }
        );
    }

    public void TurretMode()
    {
        if (target != null)
        {
            Vector3 _direction = target.position - startPoint.position; // 타겟 방향으로 레이저 발사
            ShootLaser(_direction, _direction.magnitude, onMiss: () =>
            {
                OnLaser(startPoint.position, target.position);
            }
            );
        }
        else
        {
            OffLaser();
        }
    }

    public override void Activate() // 감지모드 On
    {
        base.Activate();
        isSensorMode = true;
    }

    public override void Deactivate() // 감지모드 Off
    {
        base.Deactivate();
        isSensorMode = false;
        OffLaser();
    }

    public void OnLaser(Vector3 startPoint, Vector3 endPoint) // 레이저 작동
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);
    }

    public void OffLaser()
    {
        lineRenderer.enabled = false;
    }


    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ClearTarget()
    {
        target = null;
    }

}
