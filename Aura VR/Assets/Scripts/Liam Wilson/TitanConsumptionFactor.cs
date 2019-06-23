using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class TitanConsumptionFactor : MonoBehaviour
{
    [SerializeField] private PowerConsumer powerConsumer;
    [SerializeField] private VRTK_SlideObjectControlAction[] slideContributors;

    void Update()
    {
        float factor = 0.0f;

        foreach (VRTK_SlideObjectControlAction s in slideContributors)
        {
            factor += Mathf.Abs(s.CurrentSpeed) / s.maximumSpeed;
        }

        factor /= slideContributors.Length;
        powerConsumer.consumptionFactor = factor;
    }
}
