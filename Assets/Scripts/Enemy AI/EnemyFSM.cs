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

public class EnemyFSM : MonoBehaviour
{
    #region Variables
    public BaseState currentState;

    [Header("States")]
    public SeekState seek = new SeekState();

    [Header("Movement")]
    [Tooltip("The speed that the agent moves. This is identical to changing the speed in thenavmesh agent")]
    public float seekSpeed;
    [Tooltip("The time, in seconds, between target position updates")]
    public float targetPositionUpdateTime;

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
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        NPCgO = this.gameObject;

        MoveToState(seek);
    }

    public void MoveToState(BaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
}
