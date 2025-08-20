using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

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
    public class ButtonController : PositioningObjectController
    {
        [SerializeField]
        public TriggerDataList OnTriggerDataList;
        public override void Enter()
        {
            if (_coPositioning != null)
                StopCoroutine(_coPositioning);
            _coPositioning = StartCoroutine(CoMoveAt(To, 0.1f, () => { Exit(); }));

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
            if (_coPositioning != null)
                StopCoroutine(_coPositioning);
            _coPositioning = StartCoroutine(CoMoveAt(From));
        }
    }
}
