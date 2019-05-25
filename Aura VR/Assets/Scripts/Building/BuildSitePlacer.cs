using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSitePlacer : MonoBehaviour
{
    [SerializeField]
    private GameObject _buildSiteBeingPlaced;

    [SerializeField]
    GameObject _placementIndicator;

    public LayerMask buildMask;

    [SerializeField]
    private KeyCode placeKey;

    private bool placing = false;

    Ray _ray;
    RaycastHit _hit;

    // Start is called before the first frame update
    void Start()
    {
        _buildSiteBeingPlaced = null;
        if (_placementIndicator != null)
        {
            _placementIndicator = Instantiate(_placementIndicator);
            _placementIndicator.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(placeKey))
        {
            placing = true;
            Physics.Raycast(_ray, out _hit, 100, buildMask);
            _placementIndicator.transform.position = _hit.point;
            _placementIndicator.SetActive(true);
        }
        if (Input.GetKeyUp(placeKey) && placing)
        {
            placing = false;
            Instantiate(_buildSiteBeingPlaced, _hit.point, Quaternion.identity);
            _placementIndicator.SetActive(false);
        }
    }    
}
