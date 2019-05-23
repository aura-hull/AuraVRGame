using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPart : MonoBehaviour
{
    [SerializeField]
    private string _partName;
    [SerializeField]
    private int _numOfUsesLeft;
    public string Name
    {
        get
        {
            return _partName;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_partName == null)
            _partName = "Name Not Given";
        if (_numOfUsesLeft == 0)
            _numOfUsesLeft = 1;
    }

    public void Use()
    {
        _numOfUsesLeft--;
        if (_numOfUsesLeft <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
