using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsController : MonoBehaviour
{
    [Header("Controller Settings")]
    [SerializeField] public GameObject optionsPrefab;
    [SerializeField] public Transform parentContainer;
    [SerializeField] public CanvasGroup buttons;

    void Update()
    {
        buttons.interactable = InputBlocker.Interactible ? true : false;
    }

    public void OpenMenu()
    {
        GameObject options = Instantiate(optionsPrefab);
        options.transform.SetParent(parentContainer, false);
        GameSettings settingsScript = options.GetComponent<GameSettings>();
        if (settingsScript != null)
        {
            settingsScript.Initialize(AudioManager.instance);
        }
        InputBlocker.Interactible = false;
    }
}
