using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackedObject : MonoBehaviour
{
    private Vector3 originPosition;
    private Quaternion originRotation;
    private Vector3 originLocalScale;

    void Start()
    {
        originPosition = transform.position;
        originRotation = transform.rotation;
        originLocalScale = transform.localScale;

        AuraSceneManager.Instance.SubscribeOnSceneReset(this.ResetInstance);
    }

    public void ResetInstance()
    {
        transform.position = originPosition;
        transform.rotation = originRotation;
        transform.localScale = originLocalScale;
    }

    void OnDestroy()
    {
        AuraSceneManager.Instance.UnsubscribeOnSceneReset(this.ResetInstance);
    }
}