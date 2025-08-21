using System.Collections;
using UnityEngine;

public class CanPositionDoorController : PositioningObjectController
{
    [SerializeField]
    string[] IgnoreLayers;
    protected override void Init()
    {
        base.Init();
        foreach (string layerName in IgnoreLayers)
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer($"{layerName}"), LayerMask.NameToLayer("Door"), true);
    }
    public override void Enter()
    {
        if (_coPositioning != null)
            StopCoroutine(_coPositioning);
        _coPositioning = StartCoroutine(CoMoveAt(To));
        AudioManager.Instance.PlaySFX("Door_on");
    }
    public override void Exit()
    {
        if (_coPositioning != null)
            StopCoroutine(_coPositioning);
        _coPositioning = StartCoroutine(CoMoveAt(From));
        AudioManager.Instance.StopSFX("Door_on");
    }
}
