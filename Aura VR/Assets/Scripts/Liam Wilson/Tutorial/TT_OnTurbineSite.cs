using System.Collections;
using System.Collections.Generic;
using AuraHull.AuraVRGame;
using UnityEngine;

public class TT_OnTurbineSite : TutorialTrigger
{
    void Start()
    {
        NetworkController.OnTurbineBuildSitePlaced += ConditionMet;
    }

    private void ConditionMet()
    {
        Trigger();
    }
}
