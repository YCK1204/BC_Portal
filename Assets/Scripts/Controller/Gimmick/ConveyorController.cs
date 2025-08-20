using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConveyorController : BaseGimmickController
{
    [SerializeField]
    private Vector3 Direction = Vector3.forward;
    [Range(1f, 10.0f)]
    [SerializeField]
    private float Speed = 1f;
    [SerializeField]
    List<string> IgnoreLayerNames;

    List<Transform> _transforms = new List<Transform>();
    protected override void Init()
    {
    }
    public override void Enter()
    {
    }
    public override void Exit()
    {
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (_transforms.Contains(collision.transform))
            return;
        if (IgnoreLayerNames.Where(name => LayerMask.NameToLayer(name) == collision.gameObject.layer).Count() > 0)
            return;
        _transforms.Add(collision.transform);
    }
    private void OnCollisionExit(Collision collision)
    {
        _transforms.Remove(collision.transform);
    }
    void Clean()
    {
        _transforms.RemoveAll(t => t == null);
    }
    private void FixedUpdate()
    {
        if (_transforms.Count == 0)
            return;
        foreach (var t in _transforms)
        {
            if (t == null)
                continue;
            t.position += Direction * Speed * Time.deltaTime;
        }
        Clean();
    }
}
