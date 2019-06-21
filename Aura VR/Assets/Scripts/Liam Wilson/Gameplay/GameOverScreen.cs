using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private InputField nameInput;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text errorText;

    public Action<string, float> OnPlayerDataConfirmed;

    void Start()
    {
        AuraSceneManager.Instance.SubscribeOnSceneReset(ResetAndHide);

        AuraGameManager.Instance.gameOverScreen = this;
    }

    public void SetScoreText(float score)
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

    public void ResetAndHide()
    {
        if (nameInput != null)
        {
            nameInput.text = "";
        }

        if (scoreText != null)
        {
            scoreText.text = "---";
        }

        gameObject.SetActive(false);
    }

    public void ConfirmPlayerData()
    {
        if (nameInput == null || nameInput.text == "") return;
        if (scoreText == null || scoreText.text == "") return;

        if (ScoreboardManager.Instance.ScoresContainsName(nameInput.text))
        {
            SetErrorText("A player with this name already exists.");
            return;
        }

        OnPlayerDataConfirmed?.Invoke(nameInput.text, float.Parse(scoreText.text));
    }

    private void SetErrorText(string msg)
    {
        if (errorText != null)
        {
            errorText.text = $"* {msg}";
        }
    }

    void OnDestroy()
    {
        AuraSceneManager.Instance.UnsubscribeOnSceneReset(ResetAndHide);
    }
}
