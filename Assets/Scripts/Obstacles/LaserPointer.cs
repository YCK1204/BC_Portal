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
            // 감지 장치 모드
            Vector3 _direction = startPoint.forward; // 정면으로 레이저 발사
            ray = new Ray(startPoint.position, _direction);
            Vector3 _endPoint = startPoint.position + _direction * laserLength;

            if (Physics.Raycast(ray, out rayHit, laserLength, obstacleMask))
            {
                _endPoint = rayHit.point;

                if (rayHit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    Debug.Log("플레이어 감지");
                    // 감지 후 이벤트 추가 (ex 경고 발생, 터렛 작동, 사망 등등)
                }
            }

            OnLaser(startPoint.position, _endPoint);
        }
        else
        {
            // 터렛 모드
            if (target != null)
            {
                Vector3 _direction = target.position - startPoint.position; // 타겟 방향으로 레이저 발사
                ray = new Ray(startPoint.position, _direction);

                if (Physics.Raycast(ray, out rayHit, _direction.magnitude, obstacleMask))
                {
                    OnLaser(startPoint.position, rayHit.point);
                }
                else
                {
                    OnLaser(startPoint.position, target.position);
                }
            }
            else
            {
                OffLaser(); // 타겟 없으면 레이저 꺼짐
            }
        }
    }

    public override void Activate()
    {
        base.Activate();
        isSensorMode = true;
    }

    public override void Deactivate()
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
