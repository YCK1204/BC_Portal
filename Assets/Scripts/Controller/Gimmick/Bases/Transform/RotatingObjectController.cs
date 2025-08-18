using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RotatingObjectController : TransformGimmickController
{
    protected Coroutine _coRotating = null;
    protected override void Init() { SetRotation((AutoSetFrom) ? From : To); }
    protected void SetFromRotation() { SetRotation(From); }
    protected void SetToRotation() { SetRotation(To); }
    protected void SetRotation(Vector3 rotation)
    {
        Quaternion quaternion = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        SetRotation(quaternion);
    }
    protected void SetRotation(Quaternion quaternion)
    {
        if (IsChild)
            transform.localRotation = quaternion;
        else
            transform.rotation = quaternion;
    }
    protected IEnumerator CoRotating(Vector3 target, float callBackDelay = 0f, Action callBack = null)
    {
        Quaternion targetRot = Quaternion.Euler(target);
        Quaternion quaternion = (IsChild) ? transform.localRotation : transform.rotation;

        while (Quaternion.Angle(quaternion, targetRot) > 0.01f)
        {
            float step = Speed * Time.deltaTime;

            quaternion = Quaternion.RotateTowards(transform.rotation, targetRot, step);
            SetRotation(quaternion);
            yield return null;
        }

        SetRotation(targetRot);
        yield return new WaitForSeconds(callBackDelay);
        callBack?.Invoke();
        _coRotating = null;
    }
}
