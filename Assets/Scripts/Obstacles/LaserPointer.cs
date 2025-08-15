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
        lineRenderer.positionCount = 2;
    }
