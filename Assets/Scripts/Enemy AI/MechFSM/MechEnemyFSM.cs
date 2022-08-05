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

    [Header("Animation")]
    Animator animator;

    [Header("NPC Settings")]
    [Tooltip("Maximum health")]
    public float health;

    [Header("Movement")]
    [Tooltip("The time, in seconds, between target position updates")]
    public float updateTime;

    [HideInInspector]
    public NavMeshAgent agent;

    [HideInInspector]
    public GameObject player;

    [HideInInspector]
    public GameObject NPCgO; //NPC Game Object

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        NPCgO = this.gameObject;

        MoveToState(seek);
    }

    void Update()
    {
        
    }

    public void MoveToState(MechBaseState state)
    {
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
