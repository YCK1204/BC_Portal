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
    protected override void Init()
    {
        base.Init();
        if (AutoShuttle)
            Enter();
    }
    public override void Enter()
    {
        if (_coPositioning != null)
            StopCoroutine(_coPositioning);
        _coPositioning = StartCoroutine(CoMoveAt(To, ShuttleInterval, () => Exit()));
    }
    public override void Exit()
    {
        if (_coPositioning != null)
            StopCoroutine(_coPositioning);
        _coPositioning = StartCoroutine(CoMoveAt(From, ShuttleInterval, () => Enter()));
    }
}
