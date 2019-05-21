using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField]
    Transform _camera;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (_camera != null)
        {

            transform.LookAt(
                new Vector3(-_camera.position.x, transform.position.y, -_camera.position.z)
                );
        }
    }    
}
