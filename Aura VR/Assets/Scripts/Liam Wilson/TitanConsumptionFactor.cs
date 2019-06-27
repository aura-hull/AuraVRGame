using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class TitanConsumptionFactor : MonoBehaviour
{
    [SerializeField] private PowerConsumer powerConsumer;

    private Vector3 lastPosition;

    private void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        float factor = Mathf.Clamp01(Vector3.Distance(transform.position, lastPosition));
        lastPosition = transform.position;
        powerConsumer.consumptionFactor = factor;
    }
}
