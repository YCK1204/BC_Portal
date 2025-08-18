using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class FloorSwitchController : BaseGimmickController
{
    [SerializeField]
    Vector3 OnSwitchSize;
    [SerializeField]
    Vector3 OffSwitchSize;
    [Range(.1f, 10f)]
    [SerializeField]
    float Speed = 1f;
    [SerializeField]
    bool UseScaleInsteadOfPosition;
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
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        bool find = false;

        if (TargetLayers.Count == 0)
            find = true;
        else
            find = (TargetLayers.Where(p => other.gameObject.layer == LayerMask.NameToLayer(p)).Count() > 0);
        
        if (find)
            Enter();
    }
    private void OnTriggerExit(Collider other)
    {
        bool find = false;

        if (TargetLayers.Count == 0)
            find = true;
        else
            find = (TargetLayers.Where(p => other.gameObject.layer == LayerMask.NameToLayer(p)).Count() > 0);

        if (find)
            Exit();
    }
    public override void Enter()
    {
        _animator.SetBool(AnimControllerParamName, true);
        if (_coOnOffSwitch != null)
            StopCoroutine(_coOnOffSwitch);
        if (UseScaleInsteadOfPosition)
            _coOnOffSwitch = StartCoroutine(CoChangeByScale(OnSwitchSize));
        else
            _coOnOffSwitch = StartCoroutine(CoChangeByPosition(OnSwitchSize));
        foreach (var gimmick in linkedGimmicks)
            gimmick.Enter();
    }
    public override void Exit()
    {
        _animator.SetBool(AnimControllerParamName, false);
        if (_coOnOffSwitch != null)
            StopCoroutine(_coOnOffSwitch);
        if (UseScaleInsteadOfPosition)
            _coOnOffSwitch = StartCoroutine(CoChangeByScale(OffSwitchSize));
        else
            _coOnOffSwitch = StartCoroutine(CoChangeByPosition(OffSwitchSize));
        foreach (var gimmick in linkedGimmicks)
            gimmick.Exit();
    }
    IEnumerator CoChangeByPosition(Vector3 destination)
    {
        Vector3 startPos = (IsChild) ? transform.localPosition : transform.position;
        Vector3 direction = (destination - startPos).normalized;
        var distance = Vector3.Distance(startPos, destination);
        float moved = 0f;

        while (moved < distance)
        {
            float moveStep = Speed * Time.deltaTime;
            if (moved + moveStep > distance)
                moveStep = distance - moved;

            if (IsChild)
                transform.localPosition += direction * moveStep;
            else
                transform.position += direction * moveStep;
            moved += moveStep;

            yield return null;
        }

        if (IsChild)
            transform.localPosition = destination;
        else
            transform.position = destination;
        _coOnOffSwitch = null;
    }
    IEnumerator CoChangeByScale(Vector3 targetScale)
    {
        Vector3 startScale = transform.localScale;
        Vector3 direction = (targetScale - startScale).normalized;

        while (Vector3.Distance(transform.localScale, targetScale) > 0.001f)
        {
            transform.localScale = Vector3.MoveTowards(
                transform.localScale,
                targetScale,
                Speed * Time.deltaTime
            );

            yield return null;
        }

        transform.localScale = targetScale;
        _coOnOffSwitch = null;
    }
    bool a = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (a)
            {
                Enter();
            }
            else
            {
                Exit();
            }
            a = !a;
        }
    }
}
