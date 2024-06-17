using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteractions : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Überprüfe, ob Collider den Tag "Finish" hat
        if (other.CompareTag("Finish"))
        {
            // Rufe die WinGame-Methode der GameController-Instanz auf
            GameController.Instance.WinGame();
        }
    }
}
