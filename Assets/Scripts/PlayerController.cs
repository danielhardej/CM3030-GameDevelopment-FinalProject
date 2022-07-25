using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public float torque = 16;
    public float acceleration = 15;

    [SerializeField]
    private Vector3 movementVector;

    private new Rigidbody rigidbody;

    private FireBullet fireBullet;

    [SerializeField]
    private bool isOnFloor;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        fireBullet = GetComponent<FireBullet>();
        isOnFloor = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigidbody.AddForce(transform.forward * movementVector.z * acceleration);        
        rigidbody.AddTorque(transform.up * movementVector.x * torque);
    }

    public void Update()
    {
        Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.down * 1f, isOnFloor ? Color.green : Color.red);
    }

    public void OnMove(InputValue input)
    {
        Vector2 inputVec = input.Get<Vector2>();

        movementVector = new Vector3(inputVec.x, 0, inputVec.y);   
    }

    public void OnFire()
    {
        fireBullet.Fire();
    }
}
