using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniController : MonoBehaviour
{
    private static readonly int IsMove = Animator.StringToHash("IsMove");
    private static readonly int IsJump = Animator.StringToHash("IsJump");
    private static readonly int IsDead = Animator.StringToHash("IsDead");
    private static readonly int GetHit = Animator.StringToHash("GetHit");
    private static readonly int IsShoot = Animator.StringToHash("IsShoot");

    [SerializeField] private Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        if(animator == null)
        {
            animator = GetComponentInChildren<Animator>(true);
        }
    }

    public void Move(Vector2 obj)
    {
        animator.SetBool(IsMove, obj.magnitude > 0.5f);
    }

    public void Jump()
    {
        animator.SetBool(IsJump, true);
    }

    public void JumpEnd()
    {
        animator.SetBool(IsJump, false);
    }

    public void Dead()
    {
        animator.SetBool(IsDead, true);
    }

    public void GHit()
    {
        animator.SetBool(GetHit, true);
    }

    public void Shoot()
    {
        animator.SetBool(IsShoot, true);
    }
}
