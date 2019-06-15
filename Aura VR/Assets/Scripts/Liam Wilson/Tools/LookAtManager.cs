using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtManager
{
    private static LookAtManager _instance;
    public static LookAtManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new LookAtManager();
            return _instance;
        }
    }

    private LookAtManager()
    {

    }

    public Transform lookAtTarget;
}
