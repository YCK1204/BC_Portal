using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScalingObjectController : TransformGimmickController
{
    protected Coroutine _coScaling = null;
    protected override void Init() { SetScale((AutoSetFrom) ? From : To); }
    protected void SetFromScale() { SetScale(From); }
    protected void SetToScale() { SetScale(To); }
    protected void SetScale(Vector3 scale) { transform.localScale = scale; }
    protected IEnumerator CoScaling(Vector3 targetScale, float callBackDelay = 0f, Action callBack = null)
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

        SetScale(targetScale);
        yield return new WaitForSeconds(callBackDelay);
        callBack?.Invoke();
        transform.localScale = targetScale;
        _coScaling = null;
    }
}
