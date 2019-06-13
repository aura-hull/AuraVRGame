using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartCollector : MonoBehaviour
{
    [SerializeField] private Transform restingPoint;

    public bool available = true;

    public Transform RestingPoint
    {
        get { return restingPoint; }
    }
}
