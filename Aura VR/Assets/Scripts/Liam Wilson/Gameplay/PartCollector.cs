using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartCollector : MonoBehaviour
{
    [SerializeField] private Transform restingPoint;
    [SerializeField] private int collectorID = -1;

    public bool available;

    public int CollectorID
    {
        get { return collectorID; }
    }

    public Transform RestingPoint
    {
        get { return restingPoint; }
    }

    public void Free()
    {
        available = true;
    }
}
