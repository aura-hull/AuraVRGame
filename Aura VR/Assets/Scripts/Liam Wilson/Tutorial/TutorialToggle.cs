using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialToggle : MonoBehaviour
{
    [SerializeField] private int showOnTutorialStep = -1;

    void Start()
    {
        TutorialManager.Instance.AddToggleBehaviour(this);
        gameObject.SetActive(false);
    }

    public void Toggle(int currentTutorialStep)
    {
        gameObject.SetActive(currentTutorialStep == showOnTutorialStep);
    }
}
