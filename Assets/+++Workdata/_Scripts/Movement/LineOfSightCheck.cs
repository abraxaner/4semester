using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LineOfSightCheck : MonoBehaviour
{
    
    private GameObject targetObject; 
    private Coroutine detectPlayer; // Ref auf die laufende Coroutine

    // SerializeFields f�r den Unity-Inspektor
    [SerializeField] SimpleEnemyBehaviour seb; // Ref auf enemy behavior script
    [SerializeField] LayerMask layerCovers; // Layermaske - definiert, welche Objekte die Sichtlinie blockieren k�nnen
    [SerializeField] private float angleViewField = 60f; // Sichtfeldwinkel

    // Trigger-Event - aufgerufen, wenn ein anderer Collider den Trigger-Collider betritt
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Player") // �berpr�ft, ob der Collider zum Player geh�rt
        {
           
            targetObject = other.gameObject;  // Zielobjekt auf Player setzen
            Debug.Log("Player entering Sight");
            detectPlayer = StartCoroutine(DetectPlayer()); // Coroutine starten, um Player zu erkennen
        }
    }

    // Trigger-Event, wenn anderer Collider den Trigger-Collider verl�sst
    private void OnTriggerExit(Collider other)
    {
        
        if (other.tag == "Player") // Erneut �berpr�fen, ob Collider zum Player geh�rt
        {
            
            targetObject = null; // Refs auf das Zielobjekt l�schen
            Debug.Log("Player left Sight");
            StopCoroutine(DetectPlayer()); // Coroutine stoppen, um Player zu erkennen
        }
    }

    
    IEnumerator DetectPlayer() // Kontinuierlich �berpr�fen, ob Player in der Sichtlinie ist
    {
        
        while (true)
        {
            
            yield return new WaitForSeconds(0.5f); // 0,5 Sekunden warten, bevor die n�chste �berpr�fung passiert
            Debug.Log("Coroutine is running");

            // Richtung und Entfernung zum Zielobjekt berechnen
            Vector3 direction = targetObject.transform.position - transform.position; 
            float distance = Vector3.Distance(transform.position, targetObject.transform.position);
            
            float targetAngle = Vector3.Angle(transform.forward, direction); // Winkel zw. Vorw�rtsrichtung d. Enemys und d. Richtung Ziel berechnen


            bool isNotSeen = targetAngle > angleViewField || IsCharacterCovered(direction, distance); // �berpr�fen, ob der Player au�erhalb des Sichtfeldes oder durch ein Hindernis blockiert ist

            Debug.Log("Is Character covered?" + isNotSeen);


            // Feindliches Verhalten aktualisieren, ob der Player sichtbar ist oder nicht
            if (!isNotSeen)
            {
                seb.hasTarget = true; // Player ist sichtbar
            }
            else
            {
                seb.hasTarget = false; // Player ist nicht sichtbar
            }
        }
    }

    
    bool IsCharacterCovered(Vector3 targetDirection, float distanceToTarget) // �berpr�fe, ob das Ziel durch ein Hindernis verdeckt ist
    {
        
        RaycastHit[] hits = new RaycastHit[5]; // Array, um Raycast-Treffer zu speichern
        Ray ray = new Ray(transform.position, targetDirection); // Ray vom aktuellen Standort in Richtung Ziel erstellen
        int amountOfHits = Physics.RaycastNonAlloc(ray, hits, distanceToTarget, layerCovers); // Raycast durchf�hren und Anzahl der Treffer erhalten
        Debug.Log(amountOfHits);

        
        if (amountOfHits > 0) { return true; } // Wenn es Treffer gibt, ist das Ziel verdeckt


        return false; // Andernfalls ist das nicht
    }
}
