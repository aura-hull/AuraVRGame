using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevTools : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            TutorialManager.Instance.EndTutorial();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            TutorialManager.Instance.EndTutorial();
            AuraGameManager.Instance.EndGameplay();
        }
    }
}
