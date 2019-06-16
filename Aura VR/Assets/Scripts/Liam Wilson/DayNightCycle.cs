using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Light dayLight;
    [SerializeField] private Light nightLight;
    [SerializeField] private float fullCycleDurationSeconds = 600;
    [SerializeField] private float startAtTime = 12;
    [SerializeField] private GameObject streetLightsParent;

    private float _degreesPerSecond = 0.0f;
    private float _dayIntensity = 1.0f;
    private float _nightIntensity = 1.0f;

    public bool IsDay
    {
        get { return (transform.eulerAngles.x >= 0.0f && transform.eulerAngles.x <= 180.0f); }
    }

    public float FullCycleDurationSeconds
    {
        get { return fullCycleDurationSeconds; }
        set
        {
            fullCycleDurationSeconds = value;
            _degreesPerSecond = 360 / value;
        }
    }

    void Start()
    {
        FullCycleDurationSeconds = fullCycleDurationSeconds;

        _dayIntensity = dayLight.intensity;
        _nightIntensity = nightLight.intensity;

        transform.Rotate(Vector3.right, ((startAtTime / 24.0f) * 360.0f) - 90.0f);
        dayLight.intensity = Mathf.Lerp(0.4f, _dayIntensity, Intensity(transform.eulerAngles.x, 0.0f, 180.0f));
    }

    void Update()
    {
        transform.Rotate(Vector3.right, _degreesPerSecond * Time.deltaTime);
        dayLight.intensity = Mathf.Lerp(0.4f, _dayIntensity, Intensity(transform.eulerAngles.x, 0.0f, 180.0f));
        //nightLight.intensity = Mathf.Lerp(0.2f, _nightIntensity, Intensity(transform.eulerAngles.x, 180.0f, 360.0f));

        if (streetLightsParent == null) return;
        streetLightsParent.SetActive(!IsDay);
    }

    private float Intensity(float angle, float min, float max)
    {
        if (angle > max || angle < min) return 0.0f;

        float a = angle - min;
        float b = max - min;
        float c = a / b / 0.5f;

        if (c <= 1.0f) return c;
        else return 1.0f - (c % 1.0f);
    }
}
