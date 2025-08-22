using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LaserPointer : ObstacleBase
{
    [SerializeField] private Transform startPoint; // 포인터 시작점
    [SerializeField] Transform target; // 타겟
    [SerializeField] LayerMask obstacleMask;  // 타겟 이외 레이어마스크(벽, 장애물 등)
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float laserLength = 10f; // 레이저 길이

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
        LaserHit();
    }

    // 델리게이터 활용 굉장히 좋다
    // 콜백으로 ray에 뭔가 맞았을때는 onHit, 맞지 않았을때는 onMiss 콜백 실행
    private void ShootLaser(Vector3 direction, float distance, Action<RaycastHit> onHit = null, Action onMiss = null)
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

        UpdateLaserPosition(startPoint.position, _endPoint);
    }

    public void LaserHit()
    {
        Vector3 _direction = startPoint.forward;

        ShootLaser(_direction, laserLength,
            onHit: (hit) =>
            {
                UpdateLaserPosition(startPoint.position, startPoint.position + _direction * laserLength);

                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    Debug.Log("플레이어 감지");
                    // 경고, 사망 등 이벤트 처리
                }
            },
            onMiss: () =>
            {
                OffLaser();
            }
        );
    }
    public override void Activate() 
    {
        base.Activate();
        OnLaser();
    }

    public override void Deactivate() 
    {
        base.Deactivate();
        OffLaser();
    }

    public void OnLaser() 
    {
        lineRenderer.enabled = true;
    }

    public void OffLaser()
    {
        lineRenderer.enabled = false;
    }

    public void UpdateLaserPosition(Vector3 startPoint, Vector3 endPoint)
    {
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);

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
