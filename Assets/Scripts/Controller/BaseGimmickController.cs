using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGimmickController : MonoBehaviour
{
    protected bool _isChild = false;
    void Start() { _isChild = transform.parent != null; Init(); }
    protected virtual void Init() { }
    public virtual void Enter() { }
    public virtual void Exit() { }
}
