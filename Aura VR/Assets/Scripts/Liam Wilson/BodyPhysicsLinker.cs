using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using VRTK;

[RequireComponent(typeof(VRTK_BodyPhysics))]
public class BodyPhysicsLinker : MonoBehaviour
{
    [HideInInspector] public VRTK_BodyPhysics bodyPhysics;

    void Start()
    {
        bodyPhysics = GetComponent<VRTK_BodyPhysics>();
        BodyPhysicsManager.Instance.Link(this);
    }
}
