using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    //Tryout Grounded
    private bool isGrounded;

    // Private Variablen für Playermovement und Physik
    private Rigidbody rb; // Rigidbody
    private Vector2 inputVector; // Speichert Eingabevektor für Bewegung
    private float movingSpeed; // Bewegungsgeschwindigkeit des Players

    // Serialisierte Felder für Bewegungsgeschwindigkeit
    [SerializeField] private float defaultSpeed = 4f; // Standardgehgeschwindigkeit
    [SerializeField] private float sneakSpeed = 1f; // Schleichgeschwindigkeit
    [SerializeField] private float sprintSpeed = 8f; // Sprintgeschwindigkeit

    [SerializeField] private float jumpForce = 10f; // Springkraft

    // Serialisierte Felder für GroundCheck
    [SerializeField] private Transform transformRayStart; // Startposition für RayStart
    [SerializeField] private float rayLength = 0.5f; // Länge des Rays
    [SerializeField] private LayerMask layerGroundCheck; // Layer-Maske für GroundCheck

    // Serialisierte Felder für SlopeCheck
    [SerializeField] private float maxAngleSlope = 30f; // Maximale Neigung, auf der der Player gehen kann

    // Serialisierte Felder für Kamerasteuerung
    [SerializeField] private Transform transformCameraFollow; // Transform für die Kameraverfolgung
    [SerializeField] float rotationSensitivity = 1f; // Empfindlichkeit der Kamerarotation
    private float cameraPitch; // Vertikale Rotation der Kamera
    private float cameraRoll; // Horizontale Rotation der Kamera
    [SerializeField] private float maxCameraPitch = 80f; // Maximale Neigung der Kamera
    [SerializeField] bool invertCameraPitch = false; // Umkehren der Pitch-Steuerung

    [SerializeField] private Transform transformVisualCharacter; // Transform des visuellen Charakters für Rotation


    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody-Komponente d. Spielobjekts erhalten
        rb.freezeRotation = true; // Verhindert, dass Rigidbody rotiert

        movingSpeed = defaultSpeed; // Setze anfängliche Bewegungsgeschwindigkeit auf die Standardgeschwindigkeit

        Debug.Log("Start"); 
    }

        private void Update()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();  // Lese Mausbewegungsdelta

        // Passe die Kameraneigung basierend auf der Mausbewegung und der Invertierungseinstellung an
        if (invertCameraPitch)
        {
            cameraPitch -= mouseDelta.y * rotationSensitivity;
        }
        else
        {
            cameraPitch += mouseDelta.y * rotationSensitivity;
        }

        // Begrenze Kameraneigung auf maximal erlaubten Wert
        cameraPitch = Mathf.Clamp(cameraPitch, -maxCameraPitch, maxCameraPitch);

        // Passe cameraRoll basierend auf der Mausbewegung an
        cameraRoll += mouseDelta.x * rotationSensitivity;

        // Setze lokale Rotation des Kamera-Follow-Transforms basierend auf Pitch und Roll
        transformCameraFollow.localEulerAngles = new Vector3(cameraPitch, cameraRoll, 0f);
    }

  
    private void FixedUpdate()
    {
        // Überprüfe, ob Player auf dem aktuellen Slope gehen kann
        if (SlopeCheck())
        {
            // Berechne Bewegungsrichtung basierend auf dem Eingabevektor und der Bewegungsgeschwindigkeit
            Vector3 movementDirection =
                new Vector3(inputVector.x * movingSpeed, rb.velocity.y, inputVector.y * movingSpeed);

            // Passe Bewegungsrichtung basierend auf der Y-Rotation der Kamera an
            movementDirection = Quaternion.AngleAxis(transformCameraFollow.localEulerAngles.y, Vector3.up) * movementDirection;

            // Setze Geschwindigkeit d. rbs auf die Bewegungsrichtung
            rb.velocity = movementDirection;

            // Rotiere d. visual character, um in Bewegungsrichtung zu schauen, wenn er sich bewegt
            if (movementDirection.x != 0 && movementDirection.z != 0)
            {
                Vector3 lookDirection = movementDirection;
                lookDirection.y = 0f;
                transformVisualCharacter.rotation = Quaternion.LookRotation(lookDirection);
            }
        }
    }

    
    void OnJump() 
    {
        /* Debug.Log("JUMP!"); 
         if (GroundCheck()) // Überprüfe, ob Player auf dem Boden ist
         {
             rb.velocity = new Vector3(0f, jumpForce, 0f); // Sprungkraft anwenden
             Debug.Log("Velocity after jump: " + rb.velocity);
         }*/

        Debug.Log("JUMP!"); 
        if (isGrounded) // Überprüfe, ob Player auf dem Boden ist
        {
            rb.velocity = new Vector3(0f, jumpForce, 0f); // Sprungkraft anwenden
            Debug.Log("Velocity after jump: " + rb.velocity);
        }
    }

    void OnSneak(InputValue inputValue)
    {
        Debug.Log("Sneak!: " + inputValue.Get<float>()); 
        float isSneak = inputValue.Get<float>();

        if (Mathf.RoundToInt(isSneak) == 1)
        {
            movingSpeed = sneakSpeed; // Bewegungsgeschwindigkeit auf Schleichgeschwindigkeit setzen
        }

        else
        {
            movingSpeed = defaultSpeed; // Auf Standardgeschwindigkeit zurücksetzen
        }
    }

    void OnSprint(InputValue inputValue)
    {
        Debug.Log("Sprint!: " + inputValue.Get<float>()); 
        float isSprint = inputValue.Get<float>();

        if (Mathf.RoundToInt(isSprint) == 1)
        {
            movingSpeed = sprintSpeed; // Bewegungsgeschwindigkeit auf Sprintgeschwindigkeit setzen
        }
        else
        {
            movingSpeed = defaultSpeed; // Auf Standardgeschwindigkeit zurücksetzen
        }
    }

    void OnMove(InputValue inputValue)
    {
        inputVector = inputValue.Get<Vector2>(); // Aktualisiere den Eingabevektor mit dem neuen Eingabewert
    }

  /*  bool GroundCheck() // Methode - überprüfe, ob der Spieler auf dem Boden ist
    {
        Debug.Log("U r grounded");
        // Führe einen Raycast aus, um den Boden unter dem Player zu überprüfen
        return Physics.Raycast(transformRayStart.position, Vector3.down, rayLength, layerGroundCheck);   
    }*/

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            Debug.Log("is Grounded");
        }
    }


    bool SlopeCheck() // Methode, um zu überprüfen, ob der Slope begehbar ist
    {
        RaycastHit hit;

        // Führe einen Raycast aus, um den Neigungswinkel zu überprüfen
        Physics.Raycast(transformRayStart.position, Vector3.down, out hit, rayLength, layerGroundCheck);

        if (hit.collider != null)
        {
            float angle = Vector3.Angle(hit.normal, Vector3.up); // Berechne Winkel zw. der Normalen d. Treffers und d. Up-Vektor
            if (angle > maxAngleSlope)
            {
                return false; // Wenn d. Neigungswinkel zu steil ist, gib false zurück
            }
        }

        return true; // Slope ist begehbar
    }
}
