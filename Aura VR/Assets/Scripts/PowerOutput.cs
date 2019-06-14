using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerOutput : MonoBehaviour
{
    [SerializeField] private float maxPowerOutput = 10.0f;
    [SerializeField] private BladeAnimate bladeAnimate;
    [SerializeField] private float bladeSpeedMultiplier = 1.0f;

    private float _powerOutput;

    // Start is called before the first frame update
    void Start()
    {
        transform.forward = PowerManager.Instance.activeWindManager.OptimalRotation(transform.position);

        _powerOutput = PowerManager.Instance.CalculatePowerOutput(transform.position, transform.forward, maxPowerOutput);

        if (bladeAnimate != null)
        {
            bladeAnimate.maxRotateSpeed = _powerOutput * bladeSpeedMultiplier;
        }

        PowerManager.Instance.IncreasePowerOutput(_powerOutput);
    }
}
