/*
 * 
 * Code by: 
 *      Dimitrios Vlachos
 *      djv1@student.london.ac.uk
 *      dimitri.j.vlachos@gmail.com
 *      
 * Adapted from: https://youtu.be/rnqF6S7PfFA
 * 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField, Tooltip("This is an empty Game Object centered on the player. This serves as the point of rotation for the Camera.")]
    GameObject CameraRig;
    [ReadOnly, SerializeField, Tooltip("This is the Main Camera which must be a child of the Camera Rig.")]
    Camera mainCamera;
    // Then we need the camera's transform, which we initialise in Start()
    Transform cameraTransform;

    [Header("Rotation and Zoom")]
    [SerializeField, Tooltip("The time it takes to lerp to the new rotation position.")]
    float movementTime = 5;
    [SerializeField, Tooltip("The speed of rotation of the camera.")]
    float rotationSpeed = 0.5f;
    [SerializeField, Tooltip("The speed the camera zooms in and out at.")]
    float zoomSpeed = 1;
    [SerializeField, Tooltip("The exact amount of change made to the camera's position each frame. Varying the numbers allows you to control the angle the camera moves up and down at.")]
    Vector3 zoomAmount = new Vector3(0, (float) 2, -1);
    
    // This is our target rotation
    Quaternion newRotation;
    // This is our target zoom
    Vector3 newZoom;
    // This is the input we receive from the user
    Vector2 look_input;

    // Start is called before the first frame update
    void Start()
    {
        // We instatiate the main camera, ASSUMING the main one is attached to our player here.
        mainCamera = Camera.main;
        cameraTransform = mainCamera.transform; // We could also use: gameObject.GetComponentInChildren<Transform>();

        // We use local positions so that our rig stays relative to our player and the camera stays relative to the rig
        newRotation = CameraRig.transform.localRotation;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame, at the end of the frame.
    void LateUpdate()
    {
        Rotate();
        Zoom();

        // Now we interpolate between our current rotation and zoom to the state set in Rotate() and Zoom() for a nice smooth camera movement
        CameraRig.transform.localRotation = Quaternion.Lerp(CameraRig.transform.localRotation, newRotation, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }

    /// <summary>
    /// Method <c>Rotate</c> checks for input along the rotation axis and sets the angle accordingly
    /// </summary>
    private void Rotate()
    {
        if (look_input.x == 0) return; // Guard statement to reduce unecessary calculations
        newRotation *= Quaternion.Euler(look_input.x * rotationSpeed * Vector3.up);
    }

    /// <summary>
    /// Method <c>Zoom</c> checks for input along the zoom axis and sets co-ordinates accordingly
    /// </summary>
    private void Zoom()
    {
        if (look_input.y == 0) return; // Guard statement to reduce unecessary calculations
        newZoom += (zoomSpeed / 10) * zoomAmount * look_input.y;

        newZoom.z = Mathf.Clamp(newZoom.z, (float) -27.5, -17);
        newZoom.y = Mathf.Clamp(newZoom.y, 1, 18);

        // Lowest = (4, -13.5)
        // HIghest = (50, -59.5)
    }

    /// <summary>
    /// Method <c>OnLook</c> fires when a look key is pressed.
    /// </summary>
    public void OnLook(InputValue input)
    {
        // We collect the 2D composite input of out 'QERF' keys.
        look_input = input.Get<Vector2>();
    }
}
