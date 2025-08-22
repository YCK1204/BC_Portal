using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 추상화를 만들어서 상속관계 친구들을 잘 활용
// abstract 와 interface 적용 기준은?
[Serializable]
public abstract class BaseGimmickController : MonoBehaviour
{
    protected bool IsChild { get { return transform.parent != null; } }
    
    // Unity 이벤트가 상속관계에서 어떻게 동작하나?
    protected virtual void Start() { Init(); }
    
    protected abstract void Init();
    public abstract void Enter();
    public abstract void Exit();
}
