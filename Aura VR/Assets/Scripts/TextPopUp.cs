using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPopUp : MonoBehaviour
{
    [SerializeField]
    GameObject _text;
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
        _text.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        _text.SetActive(false);
    }
}
