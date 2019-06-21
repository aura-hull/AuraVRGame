using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private InputField nameInput;
    [SerializeField] private Text scoreText;

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

        OnPlayerDataConfirmed?.Invoke(nameInput.text, float.Parse(scoreText.text));
    }

    void OnDestroy()
    {
        AuraSceneManager.Instance.UnsubscribeOnSceneReset(ResetAndHide);
    }
}
