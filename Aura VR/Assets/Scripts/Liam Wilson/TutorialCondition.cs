using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCondition : MonoBehaviour
{
    [SerializeField] protected bool canTriggerEarly = false;

    public int tutorialIndex;
    public Action OnConditionMet;
    public bool wasTriggeredEarly { get; protected set; } = false;

    protected bool live = false;

    void Start()
    {
        TutorialManager.Instance.specialConditions.Add(this);
        //OnConditionMet += AuraGameManager.Instance.tutorialModel.PlayNextSpeaker;
    }

    public void SetLive()
    {
        live = true;
    }
}