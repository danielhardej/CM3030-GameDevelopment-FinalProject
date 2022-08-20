/*
 * 
 * Code by: 
 *      Dimitrios Vlachos
 *      djv1@student.london.ac.uk
 *      dimitri.j.vlachos@gmail.com
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(EnemyFSM))]
//[RequireComponent(typeof(Image))]
public class HealthBar_Mech : MonoBehaviour
{
    MechEnemyFSM fsm;
    float maxHealth;
    float currentHealth;

    // These values are used in the smoothstep movement of the bar
    float previousHealth;
    float targetHealth;

    [SerializeField, Tooltip("Healthbar update speed")]
    float updateSpeed = 10;

    [SerializeField, Tooltip("UI Image that is the healthbar to grow and shrink")]
    Image healthBar;

    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        // Get the FSM of the enemy this is attached to
        fsm = GetComponentInParent<MechEnemyFSM>();
        // Get the max health (assuming the enemy starts at max)
        maxHealth = fsm.health;
        // Set starting health
        currentHealth = fsm.health;
        // Set starting value
        previousHealth = fsm.health;
        // Set dynamic health
        targetHealth = fsm.health;
        // Get the main camera
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealth();
        TurnToCamera();
    }

    /// <summary>
    /// Gets the entity's current health from the FSM, then updates the
    /// visual health bar to reflect it
    /// </summary>
    void UpdateHealth()
    {
        // Get the current health
        currentHealth = fsm.health;

        // If there has been a change in health, begin moving the bar
        if (currentHealth != previousHealth)
        {
            // Each frame moves a fraction of the curve
            targetHealth = Mathf.SmoothStep(targetHealth, currentHealth, Time.deltaTime * updateSpeed);
        }
        
        // If we have fully interpolated to the current health value, reset the check for a change in values
        if (targetHealth == currentHealth)
        {
            previousHealth = currentHealth;
        }

        // Calculate the percentage
        float healthPercentage = targetHealth / maxHealth;

        //targetHealth = Mathf.SmoothStep();

        // Update bar to reflect it
        healthBar.fillAmount = healthPercentage;
    }

    /// <summary>
    /// Gets the location of the camera in this frame and turns the
    /// healthbar to face it. Keeping it visible.
    /// </summary>
    void TurnToCamera()
    {
        // Get the direction to look
        Vector3 v = cam.transform.position - transform.position;
        // Cancel the other directions of rotation
        v.x = v.z = 0.0f;

        // Turn to the camera
        transform.LookAt(cam.transform.position - v);
        transform.Rotate(0, 180, 0);
    }
}
