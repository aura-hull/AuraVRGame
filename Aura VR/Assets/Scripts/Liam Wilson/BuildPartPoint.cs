using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BuildPartPoint : MonoBehaviour
{
    public int pointIndex = -1;
    public BuildPartMatcher matcher = null;
    public Action<int> OnSuccessfulMatch;
    public Action<int> OnMatchRemoved;

    private Collider _currentOther;

    public string PartName
    {
        get
        {
            if (matcher == null) return "";
            return matcher.PartName;
        }
    }

    void Start()
    {
        Collider collider = GetComponent<Collider>();
        collider.isTrigger = true;
    }

    void OnTriggerStay(Collider other)
    {
        if (_currentOther != null) return;
        if (pointIndex == -1) return;
        if (matcher == null) return;

        BuildPartPoint otherPoint = other.GetComponent<BuildPartPoint>();
        if (otherPoint == null) return;

        BuildPartMatcher otherMatcher = otherPoint.matcher;
        if (otherMatcher == null) return;

        if (otherPoint.pointIndex == pointIndex && otherMatcher.Index == matcher.Index)
        {
            _currentOther = other;
            OnSuccessfulMatch?.Invoke(pointIndex);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (_currentOther == null) return;
        if (_currentOther != other) return;

        // _currentOther is only set if a BuildPart match is found, so no checks needed.
        _currentOther = null;
        OnMatchRemoved?.Invoke(pointIndex);
    }
}
