using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardControls : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TitanMovement moveComp = this.GetComponent<TitanMovement>();

        if (Input.GetKey(KeyCode.W)) // Move forward
            moveComp.MoveVector = new Vector3(10, 0, 0);
        else if (Input.GetKey(KeyCode.S)) // Move backwards
            moveComp.MoveVector = new Vector3(-10, 0, 0);
        else if (Input.GetKey(KeyCode.A)) // Move left
            moveComp.MoveVector = new Vector3(0, 0, 10);
        else if (Input.GetKey(KeyCode.D)) // Move right
            moveComp.MoveVector = new Vector3(0, 0, -10);
        else // Dont move
            moveComp.MoveVector = new Vector3(0, 0, 0);
    }
}
