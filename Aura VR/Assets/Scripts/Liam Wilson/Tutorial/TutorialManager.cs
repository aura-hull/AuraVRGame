using System;
using System.Collections;
using System.Collections.Generic;
using AuraHull.AuraVRGame;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class TutorialManager
{
    class ConditionPair
    {
        public TutorialTrigger trigger { get; private set; }
        public bool conditionWasMet { get; private set; }

        public int TutorialIndex
        {
            get { return trigger.tutorialIndex; }
        }

        public ConditionPair(TutorialTrigger trigger)
        {
            this.trigger = trigger;
            this.conditionWasMet = false;

            trigger.OnConditionMet += () => { conditionWasMet = true; };
        }
    }

    private static TutorialManager _instance;
    public static TutorialManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new TutorialManager();
            return _instance;
        }
    }

    public TutorialModel tutorialModel;
    private List<ConditionPair> specialConditions;
    private List<TutorialToggle> toggleBehaviours;
    public bool isRunning { get; private set; } = false;

    public Action OnTutorialStart;
    public Action OnTutorialEnd;

    private int _waitingOnCondition = -1;

    public bool WaitingOnCondition
    {
        get { return _waitingOnCondition != -1; }
    }

    private TutorialManager()
    {
        specialConditions = new List<ConditionPair>();
        toggleBehaviours = new List<TutorialToggle>();
    }

    public bool AddTriggerCondition(TutorialTrigger trigger)
    {
        specialConditions.Add(new ConditionPair(trigger));
        return true;
    }

    public bool AddToggleBehaviour(TutorialToggle toggle)
    {
        toggleBehaviours.Add(toggle);
        return true;
    }

    public void StartTutorial()
    {
        if (tutorialModel == null) return;
        
        tutorialModel.Initialize();
        isRunning = true;

        OnTutorialStart?.Invoke();
    }

    public void EndTutorial()
    {
        AuraGameManager.Instance.SetState(AuraGameManager.GameState.Gameplay);
        isRunning = false;

        OnTutorialEnd?.Invoke();
    }

    public void ToggleBehaviours()
    {
        foreach (TutorialToggle tt in toggleBehaviours)
        {
            tt.Toggle(tutorialModel.speaker.currentDialogue);
        }
    }

    public bool SpecialConditionIsDone()
    {
        bool value = specialConditions[_waitingOnCondition].conditionWasMet;
        if (value == true) _waitingOnCondition = -1;
        return value;
    }

    public void CheckNextTutorialCondition()
    {
        for (int i = 0; i < specialConditions.Count; i++)
        {
            if (specialConditions[i].TutorialIndex == tutorialModel.speaker.currentDialogue)
            {
                specialConditions[i].trigger.SetLive();
                _waitingOnCondition = i;
                return;
            }
        }
        
        NetworkController.Instance.NotifyPlayNextTutorial(tutorialModel.speaker.currentDialogue++);
    }

    public void SetPenguinVisible(bool value)
    {
        if (tutorialModel != null && tutorialModel.renderer != null)
        {
            tutorialModel.renderer.enabled = value;
        }
    }

    public void SilencePenguin()
    {
        if (tutorialModel != null)
        {
            tutorialModel.speaker.Stop();
            isRunning = false;
            OnTutorialEnd?.Invoke();
        }
    }
}
