using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class UILevel : MonoBehaviour
{
    // Serialized fields für UI-Elemente
    [SerializeField] CanvasGroup panelStart; // CanvasGroup für d. Startpanel
    [SerializeField] Button buttonStartLevel; // Button zum Starten d. Levels

    public GameObject pauseMenuUI;

    [SerializeField] CanvasGroup panelWin; // CanvasGroup für Win-Panel
    [SerializeField] Button buttonNextLevel; //  Button zum Laden d. nächsten Levels
    [SerializeField] Button buttonAgain1; // Button zum Neuladen d. Levels v. Win-panel
    [SerializeField] Button buttonBacktoMenu1; // Button zum Zurückkehren zum Menü vom Win-Panel

    [SerializeField] CanvasGroup panelLose; // CanvasGroup für das Lose-Panel
    [SerializeField] Button buttonAgain2; // Button zum Neuladen des Levels vom Lose-Panel
    [SerializeField] Button buttonBacktoMenu2; // Button zum Zurückkehren zum Menü vom Lose-Panel

    void Start()
    {
        // Verstecke die Win- und Lose-Panels zu Beginn
        panelWin.HideCanvasGroup();
        panelLose.HideCanvasGroup();

        // Füge dem Start-Level-Button einen Listener hinzu, um das Startpanel auszublenden und das Level zu starten
        buttonStartLevel.onClick.AddListener(() =>
        {
            panelStart.HideCanvasGroup();
            GameController.Instance.StartLevel();
        });

        // Füge die Buttons auf dem Win-Panel Listener hinzu
        buttonAgain1.onClick.AddListener(GameController.Instance.ReloadLevel); // Level neu laden
        buttonBacktoMenu1.onClick.AddListener(GameController.Instance.LoadMenu); // Menü laden
        buttonNextLevel.onClick.AddListener(GameController.Instance.LoadNextLevel); // Nächstes Level laden

        // Füge die Buttons auf dem Lose-Panel Listener hinzu
        buttonAgain2.onClick.AddListener(GameController.Instance.ReloadLevel); // Level neu laden
        buttonBacktoMenu2.onClick.AddListener(GameController.Instance.LoadMenu); // Menü laden
    }

    public void ShowPauseScreen()
    {
        pauseMenuUI.SetActive(true);
    }

    public void HidePauseScreen()
    {
        pauseMenuUI.SetActive(false);
    }

    public void OnContinueButton()
    {
        GameController.Instance.ContinueGame();
    }

    public void OnBackToMenuButton()
    {
        GameController.Instance.LoadMenu();
    }

    public void ShowWinScreen() // Methode, um den Win-Bildschirm anzuzeigen
    {
        panelWin.ShowCanvasGroup(); // Zeige das Win-Panel an
    }

    public void ShowLoseScreen() // Methode, um den Lose-Bildschirm anzuzeigen
    {
        panelLose.ShowCanvasGroup(); // Zeige das Lose-Panel an
    }
}


public static class ExtensionMethods // statische Klasse für Erweiterungsmethoden
{
    public static void ShowCanvasGroup(this CanvasGroup myCanvasGroup)
    {
        myCanvasGroup.alpha = 1f; // Setze die Alpha auf 1, um sie sichtbar zu machen
        myCanvasGroup.interactable = true; // Mach sie interaktiv
        myCanvasGroup.blocksRaycasts = true; // Aktiviere das Blockieren von Raycasts
    }

    public static void HideCanvasGroup(this CanvasGroup myCanvasGroup)
    {
        myCanvasGroup.alpha = 0f; // Setze die Alpha auf 0, um sie unsichtbar zu machen
        myCanvasGroup.interactable = false; // Mache sie nicht interaktiv
        myCanvasGroup.blocksRaycasts = false; /// Deaktiviere das Blockieren von Raycasts
    }
}
