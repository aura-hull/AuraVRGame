﻿using System.Collections;
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

    private TutorialManager()
    {
        specialConditions = new List<TutorialCondition>();
    }
}