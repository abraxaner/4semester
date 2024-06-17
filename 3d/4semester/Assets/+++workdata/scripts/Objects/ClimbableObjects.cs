using UnityEngine;
using UnityEngine.InputSystem;

public class ClimbableObjects : MonoBehaviour
{
    private GameInput inputActions;
    private InputAction interactAction;

    private bool canInteract, isClimbing;
    [SerializeField] private Rigidbody player;
    private Vector3 climb;
    [SerializeField] private float climbingSpeed = 4f;

    private void Awake()
    {
        inputActions = new GameInput();
        interactAction = inputActions.Player.Interact;
    }

    private void OnEnable()
    {
        inputActions.Enable();
        interactAction.performed += Interact;
    }

    private void OnDisable()
    {
        inputActions.Disable();
        interactAction.canceled -= Interact;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("entered trigger");
            canInteract = true;
            isClimbing = false;
            player.useGravity = true;
            player.GetComponent<PlayerController>().enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
        }
    }

    private void FixedUpdate()
    {
        if (isClimbing)
        {
            Climb();
        }
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (canInteract)
        {
            isClimbing = true;
            Debug.Log("can Interact");
            player.GetComponent<PlayerController>().enabled = false;
        }
    }

    private void Climb()
    {
        climb.y = Input.GetAxis("Vertical");
        player.useGravity = false;
        player.MovePosition(player.position + climb * (climbingSpeed * Time.deltaTime));
    }
}
