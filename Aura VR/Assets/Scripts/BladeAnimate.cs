using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeAnimate : MonoBehaviour
{
    private float _maxRotateSpeed = 0.0f;
    public float maxRotateSpeed
    {
        get { return _maxRotateSpeed; }
        set
        {
            if (_co != null) StopCoroutine(_co);
            _co = StartCoroutine(LerpToSpeed(value, 5.0f));
        }
    }

    private float _currentSpeed = 0.0f;
    private Coroutine _co;
    
    
    void Update()
    {
        // Rotate blades around Z
        transform.Rotate(Vector3.forward, _currentSpeed * Time.deltaTime);
    }

    private IEnumerator LerpToSpeed(float target, float durationSeconds)
    {
        float degreesPerTick = (target - _currentSpeed) / durationSeconds;

        while (durationSeconds > 0.0f)
        {
            durationSeconds -= Time.deltaTime;
            _currentSpeed += degreesPerTick * Time.deltaTime;
            yield return null;
        }

        _currentSpeed = target;
        yield return null;
    }
}
