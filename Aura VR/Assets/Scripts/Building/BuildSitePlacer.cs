using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using RootMotion;
using UnityEngine;
using VRTK;

public class BuildSitePlacer : MonoBehaviour
{
    public static int idAssignment = 0;
    public static int controllingId = -1;

    [SerializeField] private GameObject buildSiteBeingPlaced;
    [SerializeField] GameObject placementIndicator;
    [SerializeField] private Material validMaterial;
    [SerializeField] private Material invalidMaterial;
    [SerializeField] private float placeRange = 2;
    [SerializeField] private string buildLayerName;

    private bool _placing = false;
    private bool _validSpawn = false;
    private Vector3 _spawnPoint;
    private int _id = -1;

    // Start is called before the first frame update
    void Awake()
    {
        _id = idAssignment++;

        VRTK_ControllerEvents controllerEvents = GetComponent<VRTK_ControllerEvents>();
        if (controllerEvents != null)
        {
            controllerEvents.ButtonTwoPressed += StartPlacement;
            controllerEvents.ButtonTwoReleased += HaltPlacement;
        }

        if (placementIndicator != null)
        {
            placementIndicator.SetActive(false);
        }
    }

    private void StartPlacement(object sender, ControllerInteractionEventArgs e)
    {
        if (controllingId != -1) return;
        controllingId = _id;
        _placing = true;
    }

    private void HaltPlacement(object sender, ControllerInteractionEventArgs e)
    {
        if (controllingId != _id) return;
        controllingId = -1;
        _placing = false;

        if (placementIndicator)
        {
            placementIndicator.SetActive(false);
        }

        if (_validSpawn)
        {
            PhotonNetwork.InstantiateSceneObject(buildSiteBeingPlaced.name, _spawnPoint, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_placing)
        {
            // Set ray here
            Ray ray = new Ray(transform.position, transform.forward); // replace this for the hand pointer
            RaycastHit hitInfo;
            
            if (Physics.Raycast(ray, out hitInfo, placeRange))
            {
                if (placementIndicator != null)
                {
                    placementIndicator.SetActive(true);
                    placementIndicator.transform.position = hitInfo.point;
                    placementIndicator.transform.rotation = Quaternion.identity;
                }
                
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer(buildLayerName))
                {
                    _spawnPoint = hitInfo.point;
                    _validSpawn = true;
                    placementIndicator.GetComponent<Renderer>().material = validMaterial;
                }
                else
                {
                    _validSpawn = false;
                    placementIndicator.GetComponent<Renderer>().material = invalidMaterial;
                }
            }
            else
            {
                _validSpawn = false;

                if (placementIndicator != null)
                {
                    placementIndicator.SetActive(false);
                }
            }
        }
    }    
}
