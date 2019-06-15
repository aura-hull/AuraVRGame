using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSource : MonoBehaviour
{
    public float speedKmH = 0.0f;
    public float radius = 100.0f;

    public Vector3 origin
    {
        get { return transform.position; }
    }

    public Vector3 direction
    {
        get { return transform.forward; }
    }
}
