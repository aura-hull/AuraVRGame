using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    private float _acceleration;
    private float _velocity;
    [SerializeField]
    [Range(0,1)]
    private float _friction;
    private float _turnAcceleration;
    private float _turnVelocity;

    // Start is called before the first frame update
    void Start()
    {
        _acceleration = 0f;
        _velocity = 0f;
        _friction = 0.95f;
        _turnAcceleration = 0f;
        _turnVelocity = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();

        // Apply rotation
        _turnVelocity += _turnAcceleration;
        transform.Rotate(Vector3.up, _turnVelocity);

        // Apply movement
        _velocity += (_acceleration * Time.deltaTime);
        _velocity *= _friction;

        transform.position += _velocity * transform.forward * Time.deltaTime;

        // Reset acceleration from this tick
        _acceleration = 0;
        _turnAcceleration = 0;
    }

    private void HandleInput()
    {
        float moveAmount = 2f;
        float turnAmount = 2f;

        // Forwards
        if (Input.GetKey(KeyCode.W))
            _acceleration += moveAmount;
        // Backwards
        if (Input.GetKey(KeyCode.S))
            _acceleration -= moveAmount;
        // Turn left
        if (Input.GetKey(KeyCode.A))
            _turnAcceleration -= turnAmount;
        // Turn right
        if (Input.GetKey(KeyCode.D))
            _turnAcceleration += turnAmount;
    }
}
