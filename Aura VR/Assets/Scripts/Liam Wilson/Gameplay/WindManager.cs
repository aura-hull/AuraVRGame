using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WindManager : MonoBehaviour
{
    private WindSource[] _windSources;
    public float defaultSpeedKmH = 0.0f;

    public float MaxKmH
    {
        get { return _windSources[0].speedKmH; }
    }

    public float MinKmH
    {
        get { return Mathf.Min(defaultSpeedKmH, _windSources[_windSources.Length - 1].speedKmH); }
    }

    void Awake()
    {
        _windSources = GetComponentsInChildren<WindSource>();
        _windSources = _windSources.OrderByDescending(x => x.speedKmH).ToArray(); // Speeds up searching.
        Debug.Log($"{_windSources.Length} wind sources found.");

        PowerManager.Instance.activeWindManager = this;
    }

    public Vector3 OptimalRotation(Vector3 position)
    {
        WindSource windSource = FindZone(position);
        if (windSource == null) return transform.forward * -1.0f;
        return windSource.direction * -1.0f;
    }

    public float GetWindSpeedKmH(Vector3 position, Vector3 orientation)
    {
        WindSource inZone = FindZone(position);

        Vector3 checkDir = (inZone == null) ? transform.forward : inZone.direction;
        Vector3 oDiff = (orientation - checkDir);
        float oDiffValue = oDiff.magnitude / 2.0f;

        if (inZone == null)
            return defaultSpeedKmH * oDiffValue;
        return inZone.speedKmH * oDiffValue;
    }

    private WindSource FindZone(Vector3 position)
    {
        foreach (WindSource ws in _windSources)
        {
            if (Vector3.Distance(position, ws.origin) <= ws.radius)
            {
                return ws;
            }
        }

        return null;
    }
}
