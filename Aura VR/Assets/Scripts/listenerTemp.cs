using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class listenerTemp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject penguin = GameObject.Find("Penguin");
            Speaker speaker = penguin.GetComponent<Speaker>();

            speaker.Speak();
        }
    }
}
