using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //trigger winning screen
            SceneManager.LoadScene("WinScreen");
            Debug.Log("You won... Congrats.. -_-");
        }
    }
}
