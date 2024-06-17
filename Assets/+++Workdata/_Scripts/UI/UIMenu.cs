using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Eflatun.SceneReference;

public class UIMenu : MonoBehaviour
{
    // F�r den Unity-Inspektor serialisieren
    [SerializeField] private Transform parentButton; // �bergeordnete Transform, wo die Level-Buttons instanziiert werden
    [SerializeField] private GameObject prefabButtonLevel; // Prefab f�r den Level-Auswahl-Button

    void Start()
    {
        int i = 0; // Index, um das aktuelle Level zu verfolgen

        foreach (SceneReference levels in GameController.Instance.scenesLevel) // �ber jede Level-Szenenref im scenesLevel-Array des GameControllers iterieren
        {
            Button button = Instantiate(prefabButtonLevel, parentButton).GetComponent<Button>();  // Button aus Prefab instanziieren und dessen ParentObjekt setzen


            button.GetComponentInChildren<TextMeshProUGUI>().text = levels.Name; // Text des Buttons auf den Namen des Levels setzen
            int capturedIndex = i;    // Aktuellen Index f�r das Klick-Event des Buttons erfassen
            button.onClick.AddListener(() => GameController.Instance.LoadLevel(capturedIndex));   // Listener zum Klick-Event des Buttons hinzuf�gen, um das ben�tigte Level zu laden
            i++; // Index inkrementieren
        }
    }
}
