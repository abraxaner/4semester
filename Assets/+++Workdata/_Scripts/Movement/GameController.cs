using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Eflatun.SceneReference;

public class GameController : MonoBehaviour
{
    public static GameController Instance { private set; get; } // Singleton-Instanz des GameControllers

    // Szenenrefs für Menü und Levels
    public SceneReference sceneMenu; // Ref zur Menüszenen
    public SceneReference[] scenesLevel; // Array v. Refs zu den Levelszenen

    public int currentSceneIndexInList; // Index d. aktuellen Levels im scenesLevel-Array

    private void Awake()
    {
        // Wenn Instanz d. GameControllers bereits existiert, zerstöre diese Instanz
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }

        else  // Andernfalls setze dies als Instanz
        {
            Instance = this;
        }

        // Dieses Objekt beim Laden d. Szene nicht zerstören
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        // Check if ESC key is pressed to toggle pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1f)
            {
                PauseGame();
            }
            else
            {
                ContinueGame();
            }
        }
    }

    private void Start()
    {
        // Spiel zu Beginn pausieren
        Time.timeScale = 0f;
    }

    public void StartLevel()
    {

        Time.timeScale = 1f; // Spiel fortsetzen
        Cursor.lockState = CursorLockMode.None; // Cursor entsperren

        //Cursor.visible = true; //  Cursor sichtbar machen  
    }


   public void PauseGame()
    {
        Time.timeScale = 0f; //Spiel stoppen
        Cursor.lockState = CursorLockMode.None; // Cursor entsperren
        FindObjectOfType<UILevel>().ShowPauseScreen();
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f; // Spiel fortsetzen
        Cursor.lockState = CursorLockMode.None; // Cursor entsperren
        FindObjectOfType<UILevel>().HidePauseScreen();
    }

    public void WinGame()
    {

        Cursor.lockState = CursorLockMode.None; // Cursor entsperren
        FindObjectOfType<UILevel>().ShowWinScreen(); // Zeig win screen
        Time.timeScale = 0f; // Spiel pausieren

        Debug.Log("Win");
    }

    public void LoseGame()
    {
        Cursor.lockState = CursorLockMode.None; // Cursor entsperren
        FindObjectOfType<UILevel>().ShowLoseScreen(); // Zeig lose screen
        Time.timeScale = 0f; // Spiel pausieren

        Debug.Log("Lost");
    }

    public void ReloadLevel()  // Aktuelle aktive Szene neu laden
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMenu() // Menüszene laden
    {
        SceneManager.LoadScene(sceneMenu.BuildIndex);
    }

    public void LoadLevel(int listIndex)  // Methode zum Laden eines Levels nach Index
    {
        currentSceneIndexInList = listIndex; // Aktuellen Levelindex setzen
        SceneManager.LoadScene(scenesLevel[listIndex].BuildIndex);  // Spezifische Levelszenen laden
    }


    public void LoadNextLevel() // Methode zum Laden d. nächsten Levels in d. Liste
    {
        int sceneToLoad = currentSceneIndexInList + 1; // Index d. nächsten Levels berechnen

        if (sceneToLoad < scenesLevel.Length) // Wenn das nächste Level existiert, lade es
        {
            SceneManager.LoadScene(scenesLevel[sceneToLoad].BuildIndex);
        }

        else  // Andernfalls lade die Menüszenen
        {
            SceneManager.LoadScene(sceneMenu.BuildIndex);
        }
    }

}
