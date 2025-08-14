using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGimmickController : MonoBehaviour
{
    void Start() { Init(); }
    protected virtual void Init() { }
    public virtual void Enter() { }
    public virtual void Exit() { }
}
