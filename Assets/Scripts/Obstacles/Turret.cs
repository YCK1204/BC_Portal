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
    //[SerializeField] private float detectionSpeed = 5f; // 터렛 감지 속도 - 안쓸 경우 삭제
    [SerializeField] private float detectionRange = 0f; // 터렛 감지 거리
    [SerializeField] private Transform target; // 타겟(플레이어)
    [SerializeField] private LayerMask targetLayerMask = 0; // Player 레이어

    [Header("Turret Fire Settings")]
    [SerializeField] private GameObject laserPrefab; // 레이저 투사체 프리팹
    [SerializeField] private Transform firePoint; // 레이저 발사 위치
    [SerializeField] private float fireCooldown = 1f; // 레이저 발사 주기

    private float lastFireTime = 0f;

    [SerializeField] private LaserPointer laserPointer; // 레이저 포인터

    [SerializeField] private bool showGizmo = true; // 기즈모 on/off


    private void Awake()
    {
        if (laserPointer == null)
            laserPointer = GetComponent<LaserPointer>();
    }

    public override void Activate() //터렛 On
    {
        base.Activate();
        InvokeRepeating("FindTarget", 0, 0.5f);
    }

    public override void Deactivate() // 터렛 Off
    {
        base.Deactivate();
        CancelInvoke("FindTarget");
        laserPointer.ClearTarget();
    }

    private void Update()
    {
        RotateTurret();
    }

    // 가장 가까운 타겟을 탐색(추후 확장을 위해 단일이 아닌 배열로 설정)
    private void FindTarget() 
    {
        Collider[] _targetCollders = Physics.OverlapSphere(transform.position, detectionRange, targetLayerMask);  // 터렛 주변 콜라이더 탐색
        Transform _shortestTarget = null; // 가장 가까운 타겟

        // 타겟 콜라이더 탐색
        if (_targetCollders.Length > 0)
        {
            float shortestTargetDistance = Mathf.Infinity;
            foreach(Collider _targetColider in _targetCollders) // 콜라이더를 찾을 때까지 탐색 반복
            {
                float targetDistance = Vector3.SqrMagnitude(transform.position - _targetColider.transform.position); // 터렛과 타겟과의 거리
                if(shortestTargetDistance > targetDistance)
                {
                    shortestTargetDistance = targetDistance;
                    _shortestTarget = _targetColider.transform;
                }
            }
        }

        target = _shortestTarget;
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
            Quaternion _targetLookRotation = Quaternion.LookRotation(target.position);
            Vector3 _targetEuler = Quaternion.RotateTowards(turretHead.rotation, _targetLookRotation, rotationSpeed * Time.deltaTime).eulerAngles;
            turretHead.rotation = Quaternion.Euler(_targetEuler.x, _targetEuler.y, 0);

            // 타겟과 일정 각도 안에서 레이저 포인터 ON
            float _angleToTarget = Quaternion.Angle(turretHead.rotation, _targetLookRotation);

            if (_angleToTarget < 5f)
            {
                laserPointer.SetTarget(target);
                Fire();
            }
            else
            {
                laserPointer.ClearTarget();
            }
        }
    }

    public void Fire()
    {
        if (target == null || Time.time < lastFireTime + fireCooldown) return;

        Vector3 _direction = (target.position - firePoint.position).normalized; // 발사 방향
        Quaternion _rotation = Quaternion.LookRotation(_direction);
        _rotation *= Quaternion.Euler(90f,0,0); // 발사각도

        GameObject _laser = Instantiate(laserPrefab, firePoint.position, _rotation); // 레이저 프리팹 생성
        Projectile _projectile = _laser.GetComponent<Projectile>();
        if (_projectile != null)
        {
            _projectile.Init(_direction);
        }

        lastFireTime = Time.time;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmo) return;
        Gizmos.color = new Color(0,1,0,0.3f);
        Gizmos.DrawSphere(transform.position, detectionRange);
    }

}
