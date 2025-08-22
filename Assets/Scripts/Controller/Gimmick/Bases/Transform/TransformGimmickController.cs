using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TransformGimmickController : BaseGimmickController
{
    [SerializeField]
    protected Vector3 From;
    [SerializeField]
    protected Vector3 To;
    [Range(1f, 10.0f)]
    [SerializeField]
    protected float Speed = 1f;
    [SerializeField]
    protected bool AutoSetFrom = false;

    protected override void Start()
    {
        base.Start();
    }
}
