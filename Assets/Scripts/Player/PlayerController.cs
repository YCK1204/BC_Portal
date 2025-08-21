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
    [SerializeField] private float groundAcceleration = 200f; // 지상에서의 가속도
    [SerializeField] private float airAcceleration = 100f;   // 공중에서의 가속도
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
    [SerializeField] private float jumpCooldown = 0.1f;
    private float nextJumpTime;
    public float rotationSpeed = 5f;

    private PlayerInput playerInput;

    protected override void Awake()

    {
        base.Awake();
        //_rigidbody = GetComponent<Rigidbody>();
        aniController = GetComponentInChildren<AniController>();
        playerInput = GetComponent<PlayerInput>();

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
        if (grounded && !wasGrounded)
        {
            aniController.JumpEnd();
            jumpLocked = false;
        }
        wasGrounded = grounded;
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, Vector3.up) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    }


    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (!UIManager.Instance.isMenuOpen) // 메뉴 열리면 카메라 이동안함
        {
            CameraLook();
        }
    }

    void Move() // 캐릭터가 움직임
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        if (dir.sqrMagnitude > 1f) dir.Normalize(); // 대각선에서도 움직임값 1 유지
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
        aniController.Move(curMovementInput);

        //// 입력 방향 계산
        //Vector3 inputDir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        //if (inputDir.sqrMagnitude > 1f) inputDir.Normalize();

        //// 현재 수평 속도 계산
        //Vector3 horizontalVel = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);

        //// 목표 속도에 도달하지 않았을 때만 힘을 추가한다.
        //if( horizontalVel.magnitude < moveSpeed)
        //{
        //    // 플레이어가 현재 땅에 있는지 공중에 있는지 확인
        //    float currentPlayerAcceleration = IsGrounded() ? groundAcceleration : airAcceleration;
        //    // 플레이어 상태에 따라 힘을 추가
        //    _rigidbody.AddForce(inputDir * currentPlayerAcceleration);
        //}

        //aniController.Move(curMovementInput);
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
        if(context.phase == InputActionPhase.Started && IsGrounded())
        {
            
            if (IsGrounded() && !jumpLocked && Time.time > nextJumpTime)
            {
                _rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                aniController.Jump();
                jumpLocked = true;
                nextJumpTime = Time.time + jumpCooldown;
            }
            //_rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            //aniController.Jump();
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
