using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerConsumer : MonoBehaviour
{
    [Serializable]
    public struct ConsumptionEffector
    {
        public string effectorTag;
        public float consumption;
    }

    [SerializeField] private float baseConsumption = 0.0f;
    [SerializeField] private ConsumptionEffector[] groundTargets;

    private float _consumptionFactor = 1.0f;
    public float consumptionFactor
    {
        get { return _consumptionFactor; }
        set { _consumptionFactor = Mathf.Clamp01(value); }
    }

    void Start()
    {
        PowerManager.Instance.SubscribeConsumer(this);
    }

    public float ReportConsumption()
    {
        float sum = baseConsumption;

        RaycastHit hitInfo;
        Ray ray = new Ray(transform.position + (Vector3.up * 0.1f), Vector3.down);

        if (Physics.Raycast(ray, out hitInfo, 1.0f))
        {
            string tag = hitInfo.transform.tag;
            foreach (ConsumptionEffector ce in groundTargets)
            {
                if (tag == ce.effectorTag) sum += ce.consumption;
            }
        }

        return sum * consumptionFactor;
    }

    void OnDestroy()
    {
        PowerManager.Instance.UnsubscribeConsumer(this);
    }
}
