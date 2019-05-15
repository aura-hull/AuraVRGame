using System.Collections;
using System.Collections.Generic;
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
        // Ensure it has a part script
        if (part != null)
        {
            // Get the name of the part
            string partName = part.GetName();

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
                            // Set part as owned
                            _partIsOwned[i] = true;
                            // Increase the number of owned parts
                            _numOfPartsOwned++;

                            // Change part to use filled material
                            Renderer partRend = _parts[i].GetComponent<Renderer>();
                            if (partRend != null)
                            {
                                partRend.material = _filledMaterial;
                            }

                            // Destroy the GameObject used as the part
                            part.Use();
                        }
                    }
                }
            }
        }
    }
}
