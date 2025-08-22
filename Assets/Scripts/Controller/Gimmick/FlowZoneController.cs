using Autodesk.Fbx;
using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using UnityEngine;

public enum FlowZoneType
{
    Up,
    Left,
    Forward,
    Down,
    Right,
    Backward
}
public enum FlowZoneControllType
{
    Reserve,
    StopOrContinue,
}
public class FlowZoneController : BaseGimmickController
{
    [SerializeField]
    FlowZoneType FlowType;
    [SerializeField]
    Material[] FlowMatrials;
    [SerializeField]
    float ScrollingSpeed = 1.0f;
    [SerializeField]
    float PullSpeed = 1.0f;
    [SerializeField]
    Collider[] IgnoreColliders;
    [SerializeField]
    FlowZoneControllType ControllType = FlowZoneControllType.Reserve;
    [SerializeField]
    float FlowSpeed = 1.0f;

    Collider _collider;
    List<GameObject> _objectsInZone = new List<GameObject>();
    ScrollingUVs_Layers _scrollingUVs;
    Vector2[] _matFlowDirections = new Vector2[]
    {
        Vector2.down,
        Vector2.down,
        Vector2.down,
        Vector2.up,
        Vector2.up,
        Vector2.up,
    };
    Vector3[] _flowDirections = new Vector3[]
    {
        Vector3.up,
        Vector3.left,
        Vector3.forward,
        Vector3.down,
        Vector3.right,
        Vector3.back,
    };
    bool _stopOrContinue = false;
    public override void Enter()
    {
        switch (ControllType)
        {
            case FlowZoneControllType.Reserve:
                FlowType = (FlowZoneType)(((int)FlowType + 3) % 6);
                _scrollingUVs.uvAnimationRate = _matFlowDirections[(int)FlowType];
                break;
            case FlowZoneControllType.StopOrContinue:
                _stopOrContinue = !_stopOrContinue;
                if (_stopOrContinue)
                {
                    _scrollingUVs.uvAnimationRate = Vector2.zero;
                    _collider.enabled = false;
                }
                else
                {
                    _scrollingUVs.uvAnimationRate = _matFlowDirections[(int)FlowType];
                    _collider.enabled = true;
                }
                break;
        }
    }
    public override void Exit()
    {
    }
    protected override void Init()
    {
        _collider = GetComponent<Collider>();
        _scrollingUVs = GetComponent<ScrollingUVs_Layers>();
        for (int i = 0; i < _matFlowDirections.Length; i++)
            _matFlowDirections[i] *= FlowSpeed;
        _scrollingUVs.uvAnimationRate = _matFlowDirections[(int)FlowType];
        foreach (var collider in IgnoreColliders)
        {
            Physics.IgnoreCollision(_collider, collider);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        var rb = other.GetComponent<Rigidbody>();
        if (rb == null)
            return;
        // 중력장 중앙점으로 모이게 중력 없애고 적용됐던 물리 효과 제거
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        _objectsInZone.Add(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        // 다시 중력 적용
        var rb = other.GetComponent<Rigidbody>();
        rb.useGravity = true;
        _objectsInZone.Remove(other.gameObject);
    }
    private void FixedUpdate()
    {
        if (_objectsInZone.Count == 0 || _stopOrContinue == true) return;
        // forward backward -> y, x
        // left right -> y, z
        // up down -> x, z
        var center = AxisVector(_collider.bounds.center);

        foreach (var obj in _objectsInZone)
        {
            if (obj == null) continue;
            var pos = AxisVector(obj.transform.position);

            // 중력장 중앙점으로 오브젝트를 끌어당김
            Vector3 direction = (center - pos).normalized;
            float distance = Vector3.Distance(center, pos);
            if (distance > .1f)
                obj.transform.position += direction * PullSpeed * Time.fixedDeltaTime;

            // 중력장 방향으로 이동
            obj.transform.position += _flowDirections[(int)FlowType] * Time.fixedDeltaTime * ScrollingSpeed * 2f;
        }
        // 만약 삭제된 오브젝트가 있다면 리스트에서 제거
        _objectsInZone.RemoveAll(obj => obj == null);
    }
    Vector3 AxisVector(Vector3 vector)
    {
        switch (FlowType)
        {
            case FlowZoneType.Up:
            case FlowZoneType.Down:
                vector.y = 0;
                break;
            case FlowZoneType.Left:
            case FlowZoneType.Right:
                vector.x = 0;
                break;
            case FlowZoneType.Forward:
            case FlowZoneType.Backward:
                vector.z = 0;
                break;
        }
        return vector;
    }
}
