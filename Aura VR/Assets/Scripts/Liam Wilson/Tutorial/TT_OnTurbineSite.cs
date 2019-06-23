using System.Collections;
using System.Collections.Generic;
using AuraHull.AuraVRGame;
using UnityEngine;

public class TT_OnTurbineSite : TutorialTrigger
{
    protected override void Start()
    {
        base.Start();

        NetworkController.OnTurbineBuildSitePlaced += ConditionMet;
    }

    private void ConditionMet()
    {
        Trigger();
    }
}
