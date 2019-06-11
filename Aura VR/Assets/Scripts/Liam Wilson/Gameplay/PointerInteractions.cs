using System.Collections;
using System.Collections.Generic;
using AuraHull.AuraVRGame;
using Photon.Pun;
using RootMotion;
using UnityEngine;
using VRTK;

[RequireComponent(typeof(LineRenderer))]
public class PointerInteractions : MonoBehaviour
{
    // Network variables
    public static int idAssignment = 0;
    public static int controllingId = -1;

    // General variables
    [SerializeField] private float maximumPointerDistance = 50.0f;

    private LineRenderer _lineRenderer;
    private bool _pointing = false;
    private int _id = -1;

    // Building variables
    [SerializeField] private GameObject buildSitePrefab;
    [SerializeField] private GameObject placementIndicator;
    [SerializeField] private Material validMaterial;
    [SerializeField] private Material invalidMaterial;
    [SerializeField] private float placeRange = 2.0f;
    [SerializeField] private string buildLayerName;
    
    private bool _validSpawn = false;
    private Vector3 _spawnPoint;

    // Start is called before the first frame update
    void Awake()
    {
        _id = idAssignment++;

        _lineRenderer = GetComponent<LineRenderer>();

        VRTK_ControllerEvents controllerEvents = GetComponent<VRTK_ControllerEvents>();
        if (controllerEvents != null)
        {
            controllerEvents.ButtonTwoPressed += StartPointing;
            controllerEvents.ButtonTwoReleased += HaltPointing;
        }

        if (placementIndicator != null)
        {
            placementIndicator.SetActive(false);
        }
    }

    private void StartPointing(object sender, ControllerInteractionEventArgs e)
    {
        if (controllingId != -1) return;
        controllingId = _id;
        _pointing = true;
    }

    private void HaltPointing(object sender, ControllerInteractionEventArgs e)
    {
        if (controllingId != _id) return;
        controllingId = -1;
        _pointing = false;

        if (placementIndicator)
        {
            placementIndicator.SetActive(false);
        }

        if (_validSpawn)
        {
            NetworkController.Instance.NotifyBuildSitePlaced(buildSitePrefab.name, _spawnPoint);
            //PhotonNetwork.InstantiateSceneObject(buildSiteBeingPlaced.name, _spawnPoint, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Default positions
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, transform.position);

        if (_pointing)
        {
            // Set ray here
            Ray ray = new Ray(transform.position, transform.forward); // replace this for the hand pointer
            RaycastHit hitInfo;
            
            if (Physics.Raycast(ray, out hitInfo, maximumPointerDistance))
            {
                float distance = Vector3.Distance(transform.position, hitInfo.point);

                if (distance <= placeRange)
                {
                    // Placing buildings
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

                // Draw the ray to the impact position.
                _lineRenderer.SetPosition(1, hitInfo.point);
                return;
            }

            // Else just draw ray to the tip of the pointer distance.
            _lineRenderer.SetPosition(1, transform.position + (transform.forward * maximumPointerDistance));
            return;
        }
    }    
}
