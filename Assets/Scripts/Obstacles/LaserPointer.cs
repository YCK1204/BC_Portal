using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : MonoBehaviour
{
    [SerializeField] private Transform startPoint; // 포인터 시작점
    public Transform target;
    [SerializeField] private float laserLength = 20f; // 레이저 길이

    [SerializeField] private LineRenderer lineRenderer;


    void Start()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
        lineRenderer.positionCount = 2; // 레이저 포인터 점 갯수
    }

    void Update()
    {
        if(target != null)
        {
            OnLazer(startPoint.position, target.position);
        }
        else
        {
            Vector3 endPoint = startPoint.position + startPoint.forward * laserLength;
            OnLazer(startPoint.position, endPoint);
        }        
    }

    public void OnLazer(Vector3 from, Vector3 to) // 레이저 작동
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, from);
        lineRenderer.SetPosition(1, to);
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
