using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject mainMenuUI, settingsUI, gameplayUI;

    private void Awake()
    {
        SetupGameStart();
    }

    public void SetupGameStart()
    {
        mainMenuUI.SetActive(true);
        settingsUI.SetActive(false);
        gameplayUI.SetActive(false);
    }
}
