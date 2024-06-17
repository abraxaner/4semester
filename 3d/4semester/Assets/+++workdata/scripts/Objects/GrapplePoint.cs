using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplePoint : MonoBehaviour
{
    private GameInput inputActions;
    private InputAction interactAction;
    
    [SerializeField] private GameObject player;
    private bool canUse;
    
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
        Debug.Log("can go up");
        canUse = true;
    }

    private void OnTriggerExit(Collider other)
    {
        canUse = false;
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (canUse)
        {
            player.transform.position = gameObject.transform.position;
        }
    }
}
