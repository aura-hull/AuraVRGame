using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.Controllables.PhysicsBased;

public class ThrottleGauge : MonoBehaviour
{
    [SerializeField] private float maxZRotation = 90;
    [SerializeField] private BoatDriver target;

    private float _originalZ = 0.0f;

    void Start()
    {
        _originalZ = transform.localEulerAngles.z;
    }

    void Update()
    {
        if (target == null) return;

        float t = target.Throttle;

        transform.localRotation = Quaternion.Euler(
            transform.localEulerAngles.x,
            transform.localEulerAngles.y,
            _originalZ + (maxZRotation * t));
    }
}
