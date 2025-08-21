using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuttleController : PositioningObjectController
{
    [SerializeField]
    bool AutoShuttle = true;
    [SerializeField]
    float ShuttleInterval = 1.0f;

    List<GameObject> _obj = new List<GameObject>();
    protected override void Init()
    {
        base.Init();
        if (AutoShuttle)
            Enter();
    }
    Vector3 _targetPosition = Vector3.zero;
    public override void Enter()
    {
        if (_coPositioning != null)
            StopCoroutine(_coPositioning);
        _targetPosition = To;
        _coPositioning = StartCoroutine(CoMoveAt(To, ShuttleInterval, () =>
        {
            if (AutoShuttle)
                Exit();
            _targetPosition = Vector3.zero;
        }));
    }
    public override void Exit()
    {
        if (_coPositioning != null)
            StopCoroutine(_coPositioning);
        _targetPosition = From;
        _coPositioning = StartCoroutine(CoMoveAt(From, ShuttleInterval, () =>
        {
            if (AutoShuttle)
                Enter();
            _targetPosition = Vector3.zero;
        }));
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (_obj.Contains(collision.gameObject))
            return;
        _obj.Add(collision.gameObject);
    }
    private void OnCollisionExit(Collision collision)
    {
        _obj.Remove(collision.gameObject);
    }
    private void Update()
    {
        if (_targetPosition == Vector3.zero)
            return;

        if (_obj.Count == 0)
            return;
        var direction = (_targetPosition - transform.position).normalized;
        foreach (var obj in _obj)
        {
            if (obj == null)
                continue;
            obj.transform.position += direction * Time.deltaTime * Speed;
        }
        _obj.RemoveAll(obj => obj == null);
    }
}
