﻿using System.Collections;
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

    [SerializeField]
    private Material _validMaterial;
    [SerializeField]
    private Material _invalidMaterial;

    private bool placing = false;
    private bool _validSpawn = false;

    public float _placeRange = 2;
    Ray _ray;
    RaycastHit _hit;
    Vector3 _spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        if (_placementIndicator != null)
        {
            _placementIndicator = Instantiate(_placementIndicator);
            _placementIndicator.SetActive(false);
        }
        _ray = new Ray(transform.position, Vector3.down);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(placeKey))
            placing = true;
        if (Input.GetKeyUp(placeKey) && placing)
        {
            placing = false;
            if (_validSpawn)
                Instantiate(_buildSiteBeingPlaced, _spawnPoint, Quaternion.identity);
            _placementIndicator.SetActive(false);
        }

        if (placing)
        {
            // Set ray here
            _ray = new Ray(transform.position, Vector3.down); // replace this for the hand pointer

            _validSpawn = Physics.Raycast(_ray, out _hit, _placeRange, buildMask);
            _placementIndicator.SetActive(true);

            if (_validSpawn)
            {
                _placementIndicator.GetComponent<Renderer>().material = _validMaterial;

                _spawnPoint = _hit.point;
                _placementIndicator.transform.position = _hit.point;
            }
            else
            {
                _placementIndicator.GetComponent<Renderer>().material = _invalidMaterial;

                _placementIndicator.transform.position = _ray.origin + (_ray.direction * _placeRange);
            }
        }
    }    
}
