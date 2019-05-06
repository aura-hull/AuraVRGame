using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPart : MonoBehaviour
{
    [SerializeField]
    private string _partName;
    [SerializeField]
    private int _numOfUsesLeft;

    // Start is called before the first frame update
    void Start()
    {
        if (_partName == null)
            _partName = "Name Not Given";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetName()
    {
        return _partName;
    }

    public void Use()
    {
        _numOfUsesLeft--;
        if (_numOfUsesLeft <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
