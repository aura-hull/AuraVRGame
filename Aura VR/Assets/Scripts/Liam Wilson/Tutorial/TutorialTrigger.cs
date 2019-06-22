using System;
using System.Collections;
using System.Collections.Generic;
using AuraHull.AuraVRGame;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public int tutorialIndex;
    public Action OnConditionMet;

    protected bool live = false;

    void Start()
    {
        TutorialManager.Instance.AddTriggerCondition(this);
    }

    public void SetLive(bool value = true)
    {
        live = value;
    }
}