using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitanMovement : MonoBehaviour
{
    private bool IsAllowedToMove { get; set; }
    [SerializeField]
    private float _maxSpeed = 10f;

    // Used for move direction and speed
    public Vector3 MoveVector { get; set; }

    private Rigidbody rbody; // local store of titans rigidbody

    // Start is called before the first frame update
    void Start()
    {
        IsAllowedToMove = true;
        rbody = null;   
    }

    // Update is called once per frame
    void Update()
    {
        if (IsAllowedToMove)
        {
            // Ensure local rbody store
            if (rbody == null)
                rbody = this.GetComponent<Rigidbody>();

            // VR controls
            InputProcess();

            // Apply max speed
            if (MoveVector.magnitude > _maxSpeed)
                MoveVector = MoveVector.normalized * _maxSpeed;

            // Update Velocity
            rbody.velocity = MoveVector;
        }
    }

    void InputProcess()
    {
        // Left controller connected
        if (OVRInput.IsControllerConnected(OVRInput.Controller.LTrackedRemote))
        {
            // Thumbstick position
            Vector2 primaryThumb = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            primaryThumb *= 10;
            MoveVector = new Vector3(primaryThumb.x, 0, primaryThumb.y); // apply to movement
        }
    }
}
