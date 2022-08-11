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
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class MechEnemyFSM : MonoBehaviour
{
    #region Variables
    public MechBaseState currentState;

    [Header("States")]
    public MechSeekState seek = new MechSeekState();
    public MechWalkShootState walkAndShoot = new MechWalkShootState();

    [Header("Animation")]
    public Animator animator;

    [Header("NPC Settings")]
    [Tooltip("Maximum health")]
    public float health;
    [Tooltip("Maximum range of weapons")]
    public float range;
    [Tooltip("The distance the mech will attempt to approach to while firing")]
    public float preferredRange;

    [Header("Movement")]
    [Tooltip("The time, in seconds, between target position updates")]
    public float updateTime;
    [Tooltip("Vertical offset of sight-line, used for shooting")]
    public float sightHeightOffset;
    [Tooltip("The mech's target location for it's NavMeshAgent"), ReadOnly]
    public Vector3 destination;

    //[Header("Targeting")]
    //[Tooltip("Left/Right rotation angle toward target"), ReadOnly]
    //public float angleToTarget;
    //[Tooltip("Up/Down rotation angle toward target"), ReadOnly]
    //public float targetDepression;

    [HideInInspector]
    public NavMeshAgent agent;

    [HideInInspector]
    public GameObject player;

    [HideInInspector]
    public GameObject NPCgO; //NPC Game Object

    [HideInInspector]
    public Transform body;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        NPCgO = this.gameObject;

        body = transform.Find("Mech/Root/Pelvis/Body");

        MoveToState(seek);
    }

    void LateUpdate()
    {
        Vector3 targetDir = player.transform.position - transform.position;

        float angleToTarget = Vector3.Angle(targetDir, transform.forward);

        body.localRotation = Quaternion.Euler(new Vector3(angleToTarget, 180f, 0f));
    }

    public void MoveToState(MechBaseState state)
    {
        Debug.Log("Entering state: " + state);
        currentState = state;
        currentState.EnterState(this);
    }

    /// <summary>
    /// Method <c>ApplyDamage</c> Receives a message sent to this object in order to apply a weapon's damage to this entitiy.
    /// </summary>
    public void ApplyDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
