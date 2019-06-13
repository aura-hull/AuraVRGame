using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class PointerSelectable : MonoBehaviour
{
    [SerializeField] private UnityEvent OnSelected;
    [SerializeField] private UnityEvent OnDeSelected;

    public void Selected()
    {
        OnSelected?.Invoke();
    }

    public void DeSelected()
    {
        OnDeSelected?.Invoke();
    }
}
