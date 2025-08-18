using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DoorController : BaseGimmickController
{
    [SerializeField]
    Vector3 _Open;
    [SerializeField]
    Vector3 _Close;
    [SerializeField]
    bool RotateOnOpen = false;
    [SerializeField]
    bool AutoOpen = false;
    [Range(1f, 10.0f)]
    [SerializeField]
    float Speed = 1.0f;
    [SerializeField]
    string[] IgnoreLayers;

    Coroutine _coOpenClose = null;
    protected override void Init()
    {
        base.Init();

        if (RotateOnOpen)
            Speed *= 10f;

        if (RotateOnOpen)
            RotateInit();
        else
            PositionInit();

        foreach (string layerName in IgnoreLayers)
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer($"{layerName}"), LayerMask.NameToLayer("Door"), true);
    }
    void RotateInit()
    {
        Quaternion quaternion = (AutoOpen) ? Quaternion.Euler(_Open.x, _Open.y, _Open.z) 
                                            : Quaternion.Euler(_Close.x, _Close.y, _Close.z);

        if (_isChild)
            transform.localRotation = quaternion;
        else
            transform.rotation = quaternion;
    }
    void PositionInit()
    {
        Vector3 position = (AutoOpen) ? _Open : _Close;
        if (_isChild)
            transform.localPosition = position;
        else
            transform.position = position;
    }
    private void Close()
    {
        if (_coOpenClose != null)
            StopCoroutine(_coOpenClose);
        if (RotateOnOpen)
            _coOpenClose = StartCoroutine(RotateTo(_Close));
        else
            _coOpenClose = StartCoroutine(MoveToPosition(_Close));
    }
    private void Open()
    {
        if (_coOpenClose != null)
            StopCoroutine(_coOpenClose);
        if (RotateOnOpen)
            _coOpenClose = StartCoroutine(RotateTo(_Open));
        else
            _coOpenClose = StartCoroutine(MoveToPosition(_Open));
    }
    IEnumerator MoveToPosition(Vector3 destination)
    {
        Vector3 startPos = (_isChild) ? transform.localPosition : transform.position;
        Vector3 direction = (destination - startPos).normalized;
        float distance = Vector3.Distance(startPos, destination);

        float moved = 0f;
        while (moved < distance)
        {
            float moveStep = Speed * Time.deltaTime;
            if (moved + moveStep > distance)
                moveStep = distance - moved;

            if (_isChild)
                transform.localPosition += direction * moveStep;
            else
                transform.position += direction * moveStep;
            moved += moveStep;

            yield return null;
        }

        if (_isChild)
            transform.localPosition = destination;
        else
            transform.position = destination; // 마지막에 정확히 맞추기
        _coOpenClose = null;
    }
    IEnumerator RotateTo(Vector3 target)
    {
        Quaternion targetRot = Quaternion.Euler(target);
        Quaternion quaternion = (_isChild) ? transform.localRotation : transform.rotation;

        while (Quaternion.Angle(quaternion, targetRot) > 0.01f)
        {
            float step = Speed * Time.deltaTime;

            quaternion = Quaternion.RotateTowards(transform.rotation, targetRot, step);
            if (_isChild)
                transform.localRotation = quaternion;
            else
                transform.rotation = quaternion;
            yield return null;
        }

        if (_isChild)
            transform.localRotation = targetRot;
        else
            transform.rotation = targetRot; // 마지막 정확히 맞춤
        _coOpenClose = null;
    }
    public override void Enter()
    {
        base.Enter();
        Open();
    }
    public override void Exit()
    {
        base.Exit();
        Close();
    }
    bool open = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (open)
            {
                Close();
            }
            else
            {
                Open();
            }
            open = !open;
        }
    }
}
