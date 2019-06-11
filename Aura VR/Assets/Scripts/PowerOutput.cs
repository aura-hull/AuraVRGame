using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerOutput : MonoBehaviour
{
    [SerializeField] private Vector2 _powerOutputMinMax = new Vector2(1, 100);
    [SerializeField] private BladeAnimate _bladeAnimate;

    private float _powerOutput;

    // Start is called before the first frame update
    void Start()
    {
        transform.forward = PowerManager.Instance.activeWindManager.OptimalRotation(transform.position);

        _powerOutput = PowerManager.Instance.CalculatePowerOutput(transform.position, transform.forward);
        _powerOutput = Mathf.Clamp(_powerOutput, _powerOutputMinMax.x, _powerOutputMinMax.y);

        if (_bladeAnimate != null)
        {
            _bladeAnimate.maxRotateSpeed = _powerOutput;
        }

        PowerManager.Instance.IncreasePowerOutput(_powerOutput);
    }
}
