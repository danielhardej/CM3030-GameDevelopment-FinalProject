using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public EnemyFSM fsm;
    public float maxHealth;
    public float currentHealth;

    [SerializeField, Tooltip("UI Image that is the healthbar to grow and shrink")]
    Image healthBar;

    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        fsm = GetComponentInParent<EnemyFSM>();
        maxHealth = fsm.health;
        currentHealth = fsm.health;
        cam = Camera.main;

        
    }

    // Update is called once per frame
    void Update()
    {
        updateHealth();
        turnToCamera();
    }

    void updateHealth()
    {
        currentHealth = fsm.health;

        float healthPercentage = currentHealth / maxHealth;

        healthBar.fillAmount = healthPercentage;
    }

    void turnToCamera()
    {
        Vector3 v = cam.transform.position - transform.position;
        v.x = v.z = 0.0f;
        transform.LookAt(cam.transform.position - v);
        transform.Rotate(0, 180, 0);
    }
}
