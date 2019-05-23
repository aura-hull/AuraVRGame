using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilSitePlacer : MonoBehaviour
{
    GameObject _buildSiteBeingPlaced;
    [SerializeField]
    GameObject _buildSite;
    Ray _ray;
    RaycastHit _hit;
    // Start is called before the first frame update
    void Start()
    {
        _buildSiteBeingPlaced = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _buildSiteBeingPlaced = Instantiate(_buildSite);
        }
        if (Input.GetKey(KeyCode.Space)&&Physics.Raycast(_ray,out _hit))
        {
            _buildSiteBeingPlaced.transform.position = _hit.point;
            //set rotation of build site
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _buildSiteBeingPlaced = null;
        }
    }    
}
