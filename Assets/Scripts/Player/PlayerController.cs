using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : PortalableObject
{
    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curMovementInput;
    public float jumpPower;
    public LayerMask groundLayerMask;

    //private Rigidbody _rigidbody;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;

    protected AniController aniController;
    [Header("Jump")]
    private bool wasGrounded;
    private bool jumpLocked;
    private bool jumpHeld;
    [SerializeField] private float jumpCooldown = 0.1f;
    private float nextJumpTime;

    private void Awake()
    {
        base.Awake();
        //_rigidbody = GetComponent<Rigidbody>();
        aniController = GetComponentInChildren<AniController>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        wasGrounded = IsGrounded();
        jumpLocked = !wasGrounded;

    }

    private void Update()
    {
        bool grounded = IsGrounded();
        if(grounded && !wasGrounded)
        {
            aniController.JumpEnd();
            jumpLocked = false;
        }
        wasGrounded = grounded;
    }
    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        CameraLook();
    }

    void Move() // 캐릭터가 움직임
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        if (dir.sqrMagnitude > 1f) dir.Normalize(); // 대각선에서도 움직임값 1 유지
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
        aniController.Move(curMovementInput);
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }
    public void OnMove(InputAction.CallbackContext context) // 움직이는 키를 눌렀을 때 입력값을 받아옴
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();

        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            jumpHeld = true;
            if (IsGrounded() && !jumpLocked && Time.time > nextJumpTime)
            {
                _rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                aniController.Jump();
                jumpLocked = true;
                nextJumpTime = Time.time + jumpCooldown;
            }
            
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            jumpHeld = false;
        }
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for(int i = 0; i< rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }
}
