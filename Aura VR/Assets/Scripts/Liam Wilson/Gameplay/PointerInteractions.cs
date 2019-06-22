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
    [SerializeField] private float minimumPointerDistance = 0.0f;
    [SerializeField] private float maximumPointerDistance = 50.0f;

    private LineRenderer _lineRenderer;
    private bool _pointing = false;
    private int _id = -1;

    // Building variables
    [SerializeField] private bool canPlaceBuildSites = true;
    [SerializeField] private GameObject buildSitePrefab;
    [SerializeField] private GameObject placementIndicator;
    [SerializeField] private GameObject deleteIndicator;
    [SerializeField] private Material validMaterial;
    [SerializeField] private Material invalidMaterial;
    [SerializeField] private float placeRange = 2.0f;
    [SerializeField] private float deleteRange = 3.0f;
    [SerializeField] private string buildLayer;
    [SerializeField] private string deleteLayer;
    [SerializeField] private LayerMask ignoreLayers;
    
    private bool _validSpawn = false;
    private Vector3 _spawnPoint;
    private PointerSelectable _currentSelection = null;
    private PhotonView targetToDelete = null;

    // Start is called before the first frame update
    void Awake()
    {
        _id = idAssignment++;

        _lineRenderer = GetComponent<LineRenderer>();

        VRTK_ControllerEvents controllerEvents = GetComponent<VRTK_ControllerEvents>();
        if (controllerEvents != null)
        {
            // Trigger
            controllerEvents.TriggerPressed += StartPointing;
            controllerEvents.TriggerReleased += HaltPointing;

            // X/A Button
            //controllerEvents.ButtonOnePressed += StartPointing;
            //controllerEvents.ButtonOneReleased += HaltPointing;

            // Y/B Button
            //controllerEvents.ButtonTwoPressed += StartPointing;
            //controllerEvents.ButtonTwoReleased += HaltPointing;
        }

        if (!canPlaceBuildSites) return;

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

        if (!canPlaceBuildSites) return;

        if (_validSpawn)
        {
            NetworkController.Instance.NotifyBuildSitePlaced(buildSitePrefab.name, _spawnPoint);
        }

        if (targetToDelete != null)
        {
            NetworkController.Instance.NotifyBuildSiteDestroyed(targetToDelete.ViewID);
        }

        if (placementIndicator != null)
        {
            placementIndicator.SetActive(false);
        }

        if (deleteIndicator != null)
        {
            deleteIndicator.SetActive(false);
        }
    }

    private bool ObjectSelection(RaycastHit hitInfo)
    {
        PointerSelectable selection = hitInfo.transform.GetComponent<PointerSelectable>();
        if (selection == null) return false;

        _currentSelection = selection;
        _currentSelection.Selected();

        _lineRenderer.SetPosition(1, hitInfo.point);

        return true;
    }

    private enum Mode { Build, Destroy, Null };
    private Mode PointerMode(RaycastHit hitInfo)
    {
        string hitLayer = LayerMask.LayerToName(hitInfo.transform.gameObject.layer);
        float distance = Vector3.Distance(transform.position, hitInfo.point);

        if (hitLayer == deleteLayer && distance < deleteRange) return Mode.Destroy;
        else if (distance < placeRange) return Mode.Build;
        else return Mode.Null;
    }

    private void PlaceBuildSite(RaycastHit hitInfo)
    {
        if (!canPlaceBuildSites) return;

        if (placementIndicator != null)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.position = hitInfo.point;
            placementIndicator.transform.rotation = Quaternion.identity;

            if (LayerMask.LayerToName(hitInfo.transform.gameObject.layer) == buildLayer)
            {
                _validSpawn = true;
                placementIndicator.GetComponent<Renderer>().material = validMaterial;
                _spawnPoint = hitInfo.point;
            }
            else
            {
                placementIndicator.GetComponent<Renderer>().material = invalidMaterial;
            }
        }

        _lineRenderer.SetPosition(1, hitInfo.point);
    }

    private void MarkSiteForDeletion(RaycastHit hitInfo)
    {
        if (!canPlaceBuildSites) return;

        if (deleteIndicator != null)
        {
            deleteIndicator.SetActive(true);
            deleteIndicator.transform.position = hitInfo.point - transform.forward;
            targetToDelete = hitInfo.transform.gameObject.GetPhotonView();
        }

        _lineRenderer.SetPosition(1, hitInfo.point);
    }

    private void ResetIndicators()
    {
        if (_currentSelection != null)
        {
            _currentSelection.DeSelected();
            _currentSelection = null;
        }

        _validSpawn = false;
        if (placementIndicator != null)
        {
            placementIndicator.SetActive(false);
        }
        
        targetToDelete = null;
        if (deleteIndicator != null)
        {
            deleteIndicator.SetActive(false);
        }

        _lineRenderer.SetPosition(1, transform.position + (transform.forward * maximumPointerDistance));
    }

    // Update is called once per frame
    void Update()
    {
        _validSpawn = false;

        // Default positions
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, transform.position);

        if (_pointing)
        {
            RaycastHit hitInfo;
            Ray ray = new Ray(transform.position, transform.forward); // replace this for the hand pointer
            if (!Physics.Raycast(ray, out hitInfo, maximumPointerDistance, ignoreLayers.Inverse()))
            {
                ResetIndicators();
                return;
            }

            if (Vector3.Distance(transform.position, hitInfo.point) < minimumPointerDistance)
            {
                ResetIndicators();
                return;
            }

            // Object selection takes first priority.
            if (ObjectSelection(hitInfo)) return;

            Mode ptrMode = PointerMode(hitInfo);
            switch (ptrMode)
            {
                case Mode.Build:
                    PlaceBuildSite(hitInfo);
                    break;

                case Mode.Destroy:
                    MarkSiteForDeletion(hitInfo);
                    break;

                case Mode.Null:
                    ResetIndicators();
                    break;
            }
        }
    }    
}
