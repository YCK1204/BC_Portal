using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : ObstacleBase
{
    [Header("Turret Settings")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float detectionSpeed = 5f;

}
