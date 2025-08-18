using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 포탈로 이동이 가능한 오브젝트는 이 스크립트를 상속받아야 합니다.
// 이 스크립트를 상속받는 오브젝트는 콜라이더와 리지드바디를 가지고 있어야 한다.
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class PortalableObject : MonoBehaviour
{
    // 현재 포탈 개수
    private int _inPortalCount = 0;

    // 포탈 종류
    private Portal _inPortal;
    private Portal _outPortal;

    private Rigidbody _rigidbody;
    private Collider _collider;

    private static readonly Quaternion halfTurn = Quaternion.Euler(0f, 180.0f, 0f);

    // 포탈에 들어갈 때 실행하는 메서드
    // 포탈 뒤 벽 콜리전을 무시하도록 해야 한다.
    public void SetIsInPortal(Portal inPortal, Portal outPortal, Collider wallCollider)
    {
        this._inPortal = inPortal;
        this._outPortal = outPortal;

        Physics.IgnoreCollision(_collider, wallCollider);

        ++_inPortalCount;
    }

    // 포탈에서 나올 때 실행하는 메서드
    public void ExitPortal(Collider wallCollider)
    {
        Physics.IgnoreCollision(_collider, wallCollider, false);
        --_inPortalCount;
    }

    public virtual void Warp()
    {
        // 포탈의 트랜스 폼을 가져온다.
        var inTransform = _inPortal.transform;
        var outTransform = _outPortal.transform;

        // 위치 변환
        Vector3 relativePos = inTransform.InverseTransformPoint(transform.position);
        relativePos = halfTurn * relativePos;
        transform.position = outTransform.TransformPoint(relativePos);

        // 회전 변환
        Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * transform.rotation;
        relativeRot = halfTurn * relativeRot;
        transform.rotation = outTransform.rotation * relativeRot;

        // 속도 변환 (운동량 보존)
        Vector3 relativeVel = inTransform.InverseTransformDirection(_rigidbody.velocity);
        relativeVel = halfTurn * relativeVel;
        _rigidbody.velocity = outTransform.TransformDirection(relativeVel);

        // 포탈 참조 교환
        var tmp = _inPortal;
        _inPortal = _outPortal;
        _outPortal = tmp;
    }
}
