using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region input variables
    private GameInput inputActions;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction sneakAction;
    #endregion
    
    private Rigidbody rb;
    [SerializeField] private Transform transformBody;
    
    [SerializeField] private float movementspeed = 4f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float sneakspeed = 1f;
    private Vector3 movement;
    [SerializeField] private float jumpForce = 16f;
    private bool isGrounded;
    private bool isSneaking = false, isSprinting = false;

    #region camera
    [SerializeField] private Transform camTransform;
    [SerializeField] private float camSensivity = 1f;
    private float cameraPitch;
    private float cameraRoll;
    [SerializeField] private float maxCameraPitch = 80f;
    [SerializeField] private bool invertCamPitch = false;
    #endregion

    #region Unity methods
    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        inputActions = new GameInput();
        jumpAction = inputActions.Player.Jump;
        sprintAction = inputActions.Player.Sprint;
        sneakAction = inputActions.Player.Sneak;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        inputActions.Enable();
        jumpAction.performed += Jump;
        sprintAction.performed += Sprint;
        sprintAction.canceled += Sprint;
        sneakAction.performed += Sneak;
    }

    private void OnDisable()
    {
        inputActions.Disable();
        jumpAction.canceled -= Jump;
        sprintAction.performed -= Sprint;
        sprintAction.canceled -= Sprint;
        sneakAction.canceled -= Sneak;
    }

    private void Update()
    {
        RotateCam();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
#endregion
    private void Movement()
    {
        movement.z = Input.GetAxis("Vertical");
        movement.x = Input.GetAxis("Horizontal");

        //rotate the movement the same amount the cam is
        movement = Quaternion.AngleAxis(camTransform.localEulerAngles.y, Vector3.up) * movement;
        
        float actualSpeed = isSneaking? sneakspeed : isSprinting? sprintSpeed : movementspeed;
        rb.MovePosition(rb.position + movement * (actualSpeed * Time.deltaTime));

        if (movement != Vector3.zero)
        {
            Vector3 lookDirection = movement;
            lookDirection.y = 0;
            transformBody.rotation = Quaternion.LookRotation(lookDirection);
        }
    }

    private void Sprint(InputAction.CallbackContext context)
    {
        isSprinting = context.performed ? true : false;
        Debug.Log(isSprinting);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void Sneak(InputAction.CallbackContext context)
    {
        if (!isSneaking)
        {
            isSneaking = true;
            gameObject.GetComponent<CapsuleCollider>().height = 1;
            gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0, -0.44f, 0);
        }
        else
        {
            isSneaking = false;
            gameObject.GetComponent<CapsuleCollider>().height = 2;
            gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0, 0, 0);
        }
    }

    private void RotateCam()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        if (invertCamPitch)
        {
            cameraPitch = cameraPitch - mouseDelta.y * camSensivity;
        }
        else
        {
            cameraPitch = cameraPitch + mouseDelta.y * camSensivity;
        }
        cameraPitch = Mathf.Clamp(cameraPitch, -maxCameraPitch, maxCameraPitch);
        cameraRoll = cameraRoll + mouseDelta.x * camSensivity;

        camTransform.localEulerAngles = new Vector3(cameraPitch, cameraRoll, 0);
    }
}
