using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIController : MonoBehaviour
{
    public Text remainingMovesText, goal1Text, goal2Text, levelText, resultsText;
    public Image goal1Image, goal2Image, resultsPanel;
    private int currentRemainingMoves, goal1Remaining, goal2Remaining;
    private TileType goal1Type, goal2Type;
    public Button restartButton;

    public static GameplayUIController instance;

    private void Awake()
    {
        resultsPanel.gameObject.SetActive(false);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("More than one GameplayUI Controller");
        }
    }

    private void Start()
    {
        SetupButtons();
    }

    private void SetupButtons()
    {
        restartButton.onClick.AddListener(() =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        });
    }

    public void Initialize(Level level)
    {
        goal1Type = level.goal1Type;
        goal2Type = level.goal2Type;
        levelText.text = string.Format("Level {0}", level.levelID + 1);
        currentRemainingMoves = level.maxMoves;
        remainingMovesText.text = level.maxMoves.ToString();
        goal1Image.sprite = TileManager.instance.tileSymbolSprites[(int)level.goal1Type];
        goal2Image.sprite = TileManager.instance.tileSymbolSprites[(int)level.goal2Type];
        goal1Remaining = level.goal1;
        goal2Remaining = level.goal2;
        goal1Text.text = goal1Remaining.ToString();
        goal2Text.text = goal2Remaining.ToString();

    }

    internal void OnTileDestroyed(TileType type)
    {
        if (type == goal1Type)
        {
            LowerGoals(1, 0);
        }
        else if (type == goal2Type)
        {
            LowerGoals(0, 1);
        }
    }

    public void LowerRemainingMoves()
    {
        currentRemainingMoves--;
        if (currentRemainingMoves <= 0)
        {
            resultsPanel.gameObject.SetActive(true);
            resultsText.text = "You lost!";
        }
        remainingMovesText.text = currentRemainingMoves.ToString();
    }

    public void LowerGoals(int goal1LowerAmount, int goal2LowerAmount)
    {
        goal1Remaining -= goal1LowerAmount;
        goal2Remaining -= goal2LowerAmount;
        if (goal1Remaining == 0 && goal2Remaining == 0 && currentRemainingMoves > 0)
        {
            resultsPanel.gameObject.SetActive(true);
            resultsText.text = "You win!";
        }
        UpdateGoalTexts();
    }

    private void UpdateGoalTexts()
    {
        if (goal1Text == null && goal2Text == null) return;
        goal1Text.text = goal1Remaining > 0 ? goal1Remaining.ToString() : "0";
        goal2Text.text = goal2Remaining > 0 ? goal2Remaining.ToString() : "0";
    }
}
