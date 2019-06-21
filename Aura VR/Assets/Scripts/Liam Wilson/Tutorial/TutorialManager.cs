using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager
{
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

    public List<TutorialCondition> specialConditions;
    public TutorialModel tutorialModel;

    private TutorialManager()
    {
        specialConditions = new List<TutorialCondition>();
    }

    public void EndTutorial()
    {
        if (tutorialModel != null)
        {
            tutorialModel.Finish();
        }

        AuraGameManager.Instance.StartGameplay();
        CleanUp();
    }

    private void CleanUp()
    {
        for (int i = 0; i < specialConditions.Count; i++)
        {
            GameObject.Destroy(specialConditions[i]);
        }

        specialConditions.Clear();
    }
}
