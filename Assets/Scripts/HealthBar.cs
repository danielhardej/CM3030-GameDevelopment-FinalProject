/*
 * 
 * Code by: 
 *      Dimitrios Vlachos
 *      djv1@student.london.ac.uk
 *      dimitri.j.vlachos@gmail.com
 *      
 * Adapted from our FSM lecture
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(EnemyFSM))]
public class HealthBar : MonoBehaviour
{
    EnemyFSM fsm;
    float maxHealth;
    float currentHealth;

    [SerializeField, Tooltip("UI Image that is the healthbar to grow and shrink")]
    Image healthBar;

    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        // Get the FSM of the enemy this is attached to
        fsm = GetComponentInParent<EnemyFSM>();
        // Get the max health (assuming the enemy starts at max)
        maxHealth = fsm.health;
        // Set starting health
        currentHealth = fsm.health;
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

        // Calculate the percentage
        float healthPercentage = currentHealth / maxHealth;

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

        // Turn the camera
        transform.LookAt(cam.transform.position - v);
        transform.Rotate(0, 180, 0);
    }
}
