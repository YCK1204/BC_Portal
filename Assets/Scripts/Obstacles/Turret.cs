using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : ObstacleBase
{
    [Header("Turret Parts")]
    [SerializeField] private Transform turretBase; // 고정대
    [SerializeField] private Transform turretHead; // 본체

    [Header("Turret Settings")]
    [SerializeField] private float rotationSpeed = 5f; // 터렛 회전 속도
    [SerializeField] private float detectionSpeed = 5f; // 터렛 감지 속도
    [SerializeField] private float detectionRange = 0f; // 터렛 감지 거리
    [SerializeField] private Transform target; // 타겟(플레이어)
    [SerializeField] private LayerMask targetLayerMask = 0; // Player 레이어

    [SerializeField] private LaserPointer laserPointer; // 레이저 포인터

    private void Awake()
    {
        if (laserPointer == null)
            laserPointer = GetComponent<LaserPointer>();
    }

    private void Start()
    {
        InvokeRepeating("FindTarget", 0, 0.5f);
    }

    private void Update()
    {
        RotateTurret();
    }

    private void FindTarget()
    {
        Collider[] targetCollders = Physics.OverlapSphere(transform.position, detectionRange, targetLayerMask);  // 터렛 주변 콜라이더 탐색
        Transform shortestTarget = null; // 가장 가까운 타겟

        // 타겟 콜라이더 탐색
        if (targetCollders.Length > 0)
        {
            float shortestTargetDistance = Mathf.Infinity;
            foreach(Collider targetColider in targetCollders) // 콜라이더를 찾을 때까지 탐색 반복
            {
                float targetDistance = Vector3.SqrMagnitude(transform.position - targetColider.transform.position); // 터렛과 타겟과의 거리
                if(shortestTargetDistance > targetDistance)
                {
                    shortestTargetDistance = targetDistance;
                    shortestTarget = targetColider.transform;
                }
            }
        }

        target = shortestTarget;
    }

    private void RotateTurret()
    {
        if (target == null) // 대기 상태에서 회전
        {
            turretHead.Rotate(new Vector3(0, 45, 0) * Time.deltaTime);
            laserPointer.ClearTarget();
        }
        else // 타겟 감지 시 타겟을 향해 회전
        {
            Quaternion targetLookRotation = Quaternion.LookRotation(target.position);
            Vector3 targetEuler = Quaternion.RotateTowards(turretHead.rotation, targetLookRotation, rotationSpeed * Time.deltaTime).eulerAngles;

            turretHead.rotation = Quaternion.Euler(targetEuler.x, targetEuler.y, 0);

            float angleToTarget = Quaternion.Angle(turretHead.rotation, targetLookRotation);

            if (angleToTarget < 5f)
            {
                laserPointer.SetTarget(target);
            }
            else
            {
                laserPointer.ClearTarget();
            }
        }
    }
}
