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

/// <summary>
/// Class <c>MechBaseState</c> Is the base state for our AI. It is an abstract class, meaning that it cannot be instanced.
/// </summary>
public abstract class MechBaseState
{
    #region Variables
    // Protected can only be used by derivative classes (classes that inherit from this one, such as the rest of our states)
    protected NavMeshAgent agent;
    protected GameObject player;
    protected GameObject NPC;

    protected float health;
    protected float targetPositionUpdateTime;

    protected MechEnemyFSM FSM;
    #endregion

    public virtual void EnterState(MechEnemyFSM npc)
    {
        agent = npc.agent;
        player = npc.player;
        NPC = npc.NPCgO;

        health = npc.health;

        targetPositionUpdateTime = npc.updateTime;

        FSM = npc;
    }
}
