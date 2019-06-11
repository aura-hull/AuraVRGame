using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private Transform targetOverride;

    void Update()
    {
        if (targetOverride != null)
        {
            transform.LookAt(targetOverride);
            return;
        }

        transform.LookAt(LookAtManager.Instance.lookAtTarget);
    }
}
