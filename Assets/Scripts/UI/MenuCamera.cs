/*
 * Written by Emilia Hardej
 * Manages rotation of camera for Main Menu background
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MenuCamera : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private Transform target;
    
    [SerializeField]
    private float distanceFromTarget = 150.0f;

    [SerializeField]
    private float heightFromTarget = 50.0f;

    void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0);

        transform.position = target.position + new Vector3(distanceFromTarget ,heightFromTarget,0) - transform.forward * distanceFromTarget;
    }
}