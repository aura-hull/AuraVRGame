using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKTargetLinker : MonoBehaviour
{
    [SerializeField] private IKTargetHandler handler;
    [SerializeField] private SyncChildTransforms sync;

    void Awake()
    {
        if (handler == null) return;
        handler.OnIKTargetsSetup += Link;
    }

    void Link()
    {
        sync.observed.Add(handler.headTarget.transform);
        sync.observed.Add(handler.leftTarget.transform);
        sync.observed.Add(handler.rightTarget.transform);
    }

    void OnDestroy()
    {
        handler.OnIKTargetsSetup -= Link;
    }
}
