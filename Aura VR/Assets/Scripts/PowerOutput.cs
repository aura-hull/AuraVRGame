using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerOutput : MonoBehaviour
{
    private float _powerOutput;
    [SerializeField]
    private Vector2 _powerOutputMinMax = new Vector2(10, 100);
    [SerializeField]
    private Vector2 _distanceMinMax = new Vector2(1, 100);
    public Transform _positionToScaleFrom;

    // Start is called before the first frame update
    void Start()
    {
        CalculateOutput();
        PowerManager.Instance.IncreasePowerOutput(_powerOutput);
        Debug.Log("Power Output: " + _powerOutput);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CalculateOutput()
    {
        float distance = Vector3.Distance(_positionToScaleFrom.position, transform.position);
        distance = Mathf.Clamp(distance, _distanceMinMax.x, _distanceMinMax.y);
        float scale = MapValue(distance, _distanceMinMax.x, _distanceMinMax.y, 0, 1);

        _powerOutput = Mathf.Lerp(_powerOutputMinMax.x, _powerOutputMinMax.y, scale);
    }

    private float MapValue(float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        float fromAbs = from - fromMin;
        float fromMaxAbs = fromMax - fromMin;

        float normal = fromAbs / fromMaxAbs;

        float toMaxAbs = toMax - toMin;
        float toAbs = toMaxAbs * normal;

        float to = toAbs + toMin;

        return to;
    }
}
