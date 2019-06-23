using System.Collections;
using System.Collections.Generic;
using AuraHull.AuraVRGame;
using UnityEngine;

public class TT_OnTurbineBuild : TutorialTrigger
{
    void Start()
    {
        NetworkController.OnTurbineBuilt += ConditionMet;
    }

    private void ConditionMet(int buildSiteViewId, string turbinePrefabName)
    {
        Trigger();
    }
}
