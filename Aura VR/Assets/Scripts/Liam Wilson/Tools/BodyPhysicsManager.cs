using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class BodyPhysicsManager
{
    private static BodyPhysicsManager _instance;
    public static BodyPhysicsManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new BodyPhysicsManager();
            return _instance;
        }
    }

    private BodyPhysicsManager()
    {
        bodyPhysics = new List<VRTK_BodyPhysics>();
    }

    private List<VRTK_BodyPhysics> bodyPhysics;

    public void Link(BodyPhysicsLinker linker)
    {
        VRTK_BodyPhysics bp = linker.bodyPhysics;
        if (bp != null) bodyPhysics.Add(bp);
    }

    public void AddToIgnoredCollisions(GameObject obj)
    {
        for (int i = 0; i < bodyPhysics.Count; i++)
        {
            bodyPhysics[i].AddIgnoredCollision(obj);
        }
    }

    public void RemoveFromIgnoredCollisions(GameObject obj)
    {
        for (int i = 0; i < bodyPhysics.Count; i++)
        {
            bodyPhysics[i].RemoveIgnoredCollision(obj);
        }
    }
}
