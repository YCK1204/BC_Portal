using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Gimmick.Btn
{
    [Serializable]
    public enum TriggerType
    {
        Enter,
        Exit,
        Toggle
    }
    [Serializable]
    public class TriggerData
    {
        public BaseGimmickController Gc;
        public TriggerType Type = TriggerType.Enter;
        internal bool _isToggled = false;
    }

    [Serializable]
    public class TriggerDataList
    {
        public List<TriggerData> DataList = new List<TriggerData>();
    }
    public class ButtonController : ScalingObjectController
    {
        [SerializeField]
        public TriggerDataList OnTriggerDataList;
        GameObject _child;
        protected override void Init()
        {
            _child = transform.GetChild(0).gameObject;
        }
        public override void Enter()
        {
            if (_coScaling != null)
                return;
            _coScaling = StartCoroutine(CoScalingChild(To, 0, () => { _coScaling = StartCoroutine(CoScalingChild(From)); }));

            foreach (var data in OnTriggerDataList.DataList)
            {
                if (data.Gc == null)
                {
                    Debug.LogError($"ButtonController({gameObject.name}) OnTriggerDataList has a empty GimmickController");
                    continue;
                }
                if (data.Type == TriggerType.Exit)
                    data.Gc.Exit();
                else if (data.Type == TriggerType.Toggle)
                {
                    if (data._isToggled)
                        data.Gc.Enter();
                    else
                        data.Gc.Exit();
                    data._isToggled = !data._isToggled;
                }
                else
                    data.Gc.Enter();
            }
        }
        public override void Exit()
        {
            if (_coScaling != null)
                return;
            _coScaling = StartCoroutine(CoScalingChild(From));
        }
        protected IEnumerator CoScalingChild(Vector3 targetScale, float callBackDelay = 0f, Action callBack = null)
        {
            Vector3 startScale = _child.transform.localScale;
            Vector3 direction = (targetScale - startScale).normalized;

            while (Vector3.Distance(_child.transform.localScale, targetScale) > 0.001f)
            {
                var scale = _child.transform.localScale;
                scale.y = Mathf.Lerp(scale.y, targetScale.y, Speed * .1f);
                _child.transform.localScale = scale;

                yield return null;
            }

            _child.transform.localScale = targetScale;
            yield return new WaitForSeconds(callBackDelay);
            _coScaling = null;
            callBack?.Invoke();
        }
    }
}