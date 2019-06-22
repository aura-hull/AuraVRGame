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
    public bool isRunning { get; private set; } = false;

    private TutorialManager()
    {
        specialConditions = new List<ConditionPair>();
    }

    public bool AddTriggerCondition(TutorialTrigger trigger)
    {
        specialConditions.Add(new ConditionPair(trigger));
        return true;
    }

    public void StartTutorial()
    {
        if (tutorialModel == null) return;
        
        tutorialModel.Initialize();
        isRunning = true;
    }

    public void EndTutorial()
    {
        AuraGameManager.Instance.SetState(AuraGameManager.GameState.Gameplay);
        isRunning = false;
    }

    public void CheckNextTutorialCondition()
    {
        foreach (ConditionPair cp in specialConditions)
        {
            if (cp.TutorialIndex == tutorialModel.speaker.currentDialogue)
            {
                cp.trigger.SetLive();
                return;
            }
        }
        
        NetworkController.Instance.NotifyPlayNextTutorial(tutorialModel.speaker.currentDialogue++);
    }
}
