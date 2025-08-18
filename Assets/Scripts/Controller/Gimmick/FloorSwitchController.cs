using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FloorSwitchController : ScalingObjectController
{
    [SerializeField]
    BaseGimmickController[] linkedGimmicks;
    [SerializeField]
    List<string> TargetLayers;
    [SerializeField]
    string AnimControllerParamName;

    Coroutine _coOnOffSwitch = null;
    Animator _animator;
    protected override void Init()
    {
        base.Init();
        _animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        OnTriggerAction(other, Enter);
    }
    private void OnTriggerExit(Collider other)
    {
        OnTriggerAction(other, Exit);
    }
    void OnTriggerAction(Collider other, Action callBack)
    {
        bool find = false;
        find |= TargetLayers.Count == 0;
        find |= (TargetLayers.Where(p => other.gameObject.layer == LayerMask.NameToLayer(p)).Count() > 0);

        if (find)
        {
            if (_coOnOffSwitch != null)
                StopCoroutine(_coOnOffSwitch);
            callBack.Invoke();
        }
    }
    public override void Enter()
    {
        _animator.SetBool(AnimControllerParamName, true);
        _coOnOffSwitch = StartCoroutine(CoScaling(From));
        foreach (var gimmick in linkedGimmicks)
            gimmick.Enter();
    }
    public override void Exit()
    {
        _animator.SetBool(AnimControllerParamName, false);
        _coOnOffSwitch = StartCoroutine(CoScaling(To));
        foreach (var gimmick in linkedGimmicks)
            gimmick.Exit();
    }
}
