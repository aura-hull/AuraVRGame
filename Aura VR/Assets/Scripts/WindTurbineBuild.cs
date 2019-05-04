using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTurbineBuild : MonoBehaviour
{
    [SerializeField]
    List<GameObject> _realBuildParts;
    [SerializeField]
    List<GameObject> _holoBuildParts;
    [SerializeField]
    GameObject _objectToCreate;
    int _numberOfBuiltParts = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        foreach (GameObject part in _holoBuildParts)
        {
            if (other.gameObject.name == part.name && part.activeSelf == true)
            {
                part.SetActive(false);
                Destroy(other.gameObject);
                foreach(GameObject realPart in _realBuildParts)
                {
                    if (realPart.name == part.name)
                    {
                        realPart.SetActive(true);
                        _numberOfBuiltParts++;
                        if (_numberOfBuiltParts >= _realBuildParts.Count)
                        {
                            Instantiate(_objectToCreate);
                            Destroy(gameObject);
                        }
                    }
                }
            }
        }
    }
}
