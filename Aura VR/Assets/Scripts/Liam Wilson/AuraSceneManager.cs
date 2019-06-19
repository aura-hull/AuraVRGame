using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AuraSceneManager
{
    private static AuraSceneManager _instance;
    public static AuraSceneManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new AuraSceneManager();
            return _instance;
        }
    }

    private AuraSceneManager() { }

    private event Action OnSceneReset;
    public void SubscribeOnSceneReset(Action action) { OnSceneReset += action; }
    public void UnsubscribeOnSceneReset(Action action) { OnSceneReset -= action; }

    public void SceneReset()
    {
        OnSceneReset?.Invoke();
    }
}
