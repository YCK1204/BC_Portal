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
    [SerializeField] private Transform target; // 타겟(플레이어)

}
