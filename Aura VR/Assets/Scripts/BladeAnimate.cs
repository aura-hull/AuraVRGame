using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeAnimate : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed = 30;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate blades around Z
        transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
    }
}
