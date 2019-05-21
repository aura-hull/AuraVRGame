using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    private float _acceleration;
    private float _velocity;
    [SerializeField]
    [Range(0,1)]
    private float _moveFriction = 0.95f;
    [SerializeField]
    [Range(0, 1)]
    private float _turnFriction = 0.6f;
    private float _turnAcceleration;
    private float _turnVelocity;
    [SerializeField]
    private float _moveAmount = 10f;
    [SerializeField]
    private float _turnAmount = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        _acceleration = 0f;
        _velocity = 0f;
        _turnAcceleration = 0f;
        _turnVelocity = 0f;

        //_moveAmount = 10f;
        //_turnAmount = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();

        // Apply rotation
        _turnVelocity += _turnAcceleration;
        transform.Rotate(Vector3.up, _turnVelocity);
        _turnVelocity *= _turnFriction;

        // Apply movement
        _velocity += (_acceleration * Time.deltaTime);
        _velocity *= _moveFriction;

        transform.position += _velocity * transform.forward * Time.deltaTime;

        // Reset acceleration from this tick
        _acceleration = 0;
        _turnAcceleration = 0;
    }

    private void HandleInput()
    {
        // Forwards
        if (Input.GetKey(KeyCode.W))
            _acceleration += _moveAmount;
        // Backwards
        if (Input.GetKey(KeyCode.S))
            _acceleration -= _moveAmount;
        // Turn left
        if (Input.GetKey(KeyCode.A))
            _turnAcceleration -= _turnAmount;
        // Turn right
        if (Input.GetKey(KeyCode.D))
            _turnAcceleration += _turnAmount;
    }
}
