using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenProjector : MonoBehaviour
{
    [SerializeField] private Transform screenCanvas;
    [SerializeField] private float cosineThreshhold = 0.0f;
    
    public bool ScreenIsActive
    {
        get { return screenCanvas.gameObject.activeSelf; }
    }

    void Start()
    {
        if (screenCanvas == null) return;
        screenCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        if (screenCanvas == null) return;
        if (screenCanvas.gameObject.activeSelf == false) return;
        if (UsefulFuncs.CosineSimilarity(transform.up, Vector3.up) >= cosineThreshhold) return;

        screenCanvas.gameObject.SetActive(false);
    }

    public void Enable(bool value)
    {
        if (screenCanvas == null) return;
        if (ScreenIsActive == value) return;

        if (UsefulFuncs.CosineSimilarity(transform.up, Vector3.up) < cosineThreshhold) return;
        screenCanvas.gameObject.SetActive(value);
    }
}
