using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : MonoBehaviour
{
    [SerializeField] private Transform startPoint; // 포인터 시작점
    [SerializeField] Transform target; // 타겟
    [SerializeField] LayerMask obstacleMask;  // 타겟 이외 오브젝트들(벽, 장애물 등)
    [SerializeField] private LineRenderer lineRenderer;

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
        if(target != null)
        {
            Vector3 _direction = target.position - startPoint.position;
            ray = new Ray(startPoint.position, _direction);

            // 벽이나 장애물 뒤에 타겟이 숨을 경우 레이저가 벽, 장애물까지만 표시되도록 경로 제한
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
            OffLaser();
        }        
    }

    public void OnLaser(Vector3 from, Vector3 to) // 레이저 작동
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, from);
        lineRenderer.SetPosition(1, to);
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
