using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockTransform : MonoBehaviour
{
    [SerializeField] private bool lockLocalPosition = true;
    [SerializeField] private Vector3 lockLocalPositionValue = Vector3.zero;
    [SerializeField] private bool lockLocalRotation = false;
    [SerializeField] private Vector3 lockLocalRotationValue = Vector3.zero;
    
    void Update()
    {
        if (lockLocalPosition) transform.localPosition = lockLocalPositionValue;
        if (lockLocalRotation) transform.localRotation = Quaternion.Euler(lockLocalRotationValue);
    }
}
