using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenProjectorView : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;

    private ScreenProjector _activeView;

    void Update()
    {
        RaycastHit hitInfo;
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out hitInfo, 100.0f, targetLayer))
        {
            Rigidbody other = hitInfo.collider.attachedRigidbody;
            _activeView = other.GetComponent<ScreenProjector>();

            if (_activeView == null) return;
            _activeView.Enable(true);
        }
        else if (_activeView != null)
        {
            _activeView.Enable(false);
            _activeView = null;
        }
    }
}
