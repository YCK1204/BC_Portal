using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class PositioningObjectController : TransformGimmickController
{
    protected Coroutine _coPositioning = null;
    protected override void Init() { SetPosition((AutoSetFrom) ? From : To); }
    protected void SetFromPosition() { SetPosition(From); }
    protected void SetToPosition() { SetPosition(To); }
    protected void SetPosition(Vector3 position)
    {
        if (IsChild)
            transform.localPosition = position;
        else
            transform.position = position;
    }
    protected IEnumerator CoMoveAt(Vector3 destination, float callBackDelay = 0f, Action callBack = null)
    {
        Vector3 start = (IsChild) ? transform.localPosition : transform.position;
        Vector3 direction = (destination - start).normalized;
        float distance = Vector3.Distance(start, destination);

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

        SetPosition(destination);
        yield return new WaitForSeconds(callBackDelay);
        callBack?.Invoke();
        _coPositioning = null;
    }
}
