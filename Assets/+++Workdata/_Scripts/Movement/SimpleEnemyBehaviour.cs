using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class SimpleEnemyBehaviour : MonoBehaviour
{
    // Private variables
    private NavMeshAgent agent; // NavMeshAgent-Komponente für Navigation
    [SerializeField] private Transform target; // Ziel-Transform, dem gefolgt werden soll

    // Serialized fields
    [SerializeField] bool isRotating = false; // ob sich der Feind drehen soll
    [SerializeField] float rotationSpeed = 20f; // Drehgeschwindigkeit
    public bool hasTarget; // ob der Feind ein Ziel hat

    void Start()
    {
        // Finde das Playerobjekt anhand des Tags und setze es als Ziel
        target = GameObject.FindGameObjectWithTag("Player").transform;
        // Hole die NavMeshAgent-Komponente, die diesem Spielobjekt angehängt ist
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Setze das Ziel des Agents auf Position des Ziels, falls hasTarget true ist
        if (hasTarget)
        {
            agent.SetDestination(target.position);
        }

        // Drehe den Enemy, falls isRotating true ist
        if (isRotating)
        {
            transform.Rotate(transform.up, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // Überprüfe, ob das kollidierte Objekt den Tag "Player" hat
        if (other.collider.tag == "Player")
        {
            Debug.Log("Lost the game");
            // Rufe die LoseGame-Methode der GameController-Instanz auf
            GameController.Instance.LoseGame();
        }
    }
}
