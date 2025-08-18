using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGimmickController : MonoBehaviour
{
    protected bool IsChild { get { return transform.parent != null; } }
    void Start() { Init(); }
    protected abstract void Init();
    public abstract void Enter();
    public abstract void Exit();
}
