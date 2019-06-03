using System.Collections;
using System.Collections.Generic;
using AuraHull.AuraVRGame;
using Photon.Pun;
using UnityEngine;

public class BuildingSite : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _parts;
    private bool[] _partIsOwned;
    private int _numOfPartsOwned;
    [SerializeField]
    GameObject _objectToBecome;
    [SerializeField]
    Material _holoMaterial;
    [SerializeField]
    Material _filledMaterial;

    void Awake()
    {
        NetworkController.OnTurbinePartBuilt += ConstructPartNetworked;
    }

    // Start is called before the first frame update
    void Start()
    {
        _numOfPartsOwned = 0;
        if (_parts == null)
            _parts = new GameObject[0];
        _partIsOwned = new bool[_parts.Length];

        for (int i = 0; i < _parts.Length; i++)
        {
            if (_parts[i] != null)
            { 
                // Change part to use holographic material
                Renderer partRend = _parts[i].GetComponent<Renderer>();
                if (partRend != null)
                {
                    partRend.material = _holoMaterial;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If the the build is complete, delete the site and make the build
        if (_numOfPartsOwned >= _parts.Length)
        {
            Instantiate(_objectToBecome,gameObject.transform.position,gameObject.transform.rotation);
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision with: " + other.name);

        BuildPart part = other.GetComponent<BuildPart>();
        if (part == null) return;

        if (ConstructPart(part.Name))
        {
            NetworkController.Instance.NotifyTurbinePartBuilt(part.Name);
            part.Use(); // Destroy the GameObject used as the part
        }
    }

    private void ConstructPartNetworked(int actorNumber, string partName)
    {
        if (actorNumber == PhotonNetwork.LocalPlayer.ActorNumber) return;
        Debug.Log("netodfsdfsdfsdf");
        ConstructPart(partName);
    }

    private bool ConstructPart(string partName)
    {
        // Ensure it has a part script
        for (int i = 0; i < _parts.Length; i++)
        {
            if (_parts[i] != null)
            {
                // Check if the part is wanted
                if (partName == _parts[i].name)
                {
                    // Check the part isn't already owned
                    if (_partIsOwned[i] == false)
                    {
                        _partIsOwned[i] = true; // Set part as owned
                        _numOfPartsOwned++; // Increase the number of owned parts

                        // Change part to use filled material
                        Renderer partRend = _parts[i].GetComponent<Renderer>();
                        if (partRend != null)
                        {
                            partRend.material = _filledMaterial;
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }
}
