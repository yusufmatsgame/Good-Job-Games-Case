using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{

    public Button playButton;


    private void Start()
    {
        SetupButtons();
    }

    private void SetupButtons()
    {
        playButton.onClick.AddListener(() =>
        {
            GameplayUIController.instance.Initialize(LevelManager.instance.levels[0]);
        });
    }
}
