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

    protected Animator animator;

    protected float health;
    protected float range;
    protected float preferredRange;
    protected float targetPositionUpdateTime;
    protected float sightHeightOffset;
    protected Vector3 destination;

    protected MechEnemyFSM FSM;
    #endregion

    public virtual void EnterState(MechEnemyFSM npc)
    {
        agent = npc.agent;
        player = npc.player;
        NPC = npc.NPCgO;

        animator = npc.animator;

        health = npc.health;

        range = npc.range;
        preferredRange = npc.preferredRange;

        targetPositionUpdateTime = npc.updateTime;
        sightHeightOffset = npc.sightHeightOffset;

        destination = npc.destination;

        FSM = npc;
    }

    /// <summary>
    /// Method <c>GetDistanceToPlayer</c> Returns the distance from this npc to the player character.
    /// </summary>
    protected float GetDistanceToPlayer()
    {
        float distance = Vector3.Distance(NPC.transform.position, player.transform.position);

        return distance;
    }

    /// <summary>
    /// Method <c>GetDirectionToPlayer</c> Returns a normalized vector direction toward the player.
    /// </summary>
    protected Vector3 GetDirectionToPlayer()
    {
        Vector3 direction = (player.transform.position - NPC.transform.position).normalized;

        return direction;
    }

    /// <summary>
    /// Method <c>HasLineOfSight</c> Checks if the NPC has line of sight with the player
    /// </summary>
    protected bool HasLineOfSight()
    {
        float distanceToPlayer = GetDistanceToPlayer();
        //Debug.Log("Distance: " + distanceToPlayer);

        // If the player is within shooting range
        if (distanceToPlayer <= range)
        {
            //Debug.Log("Within range");
            // Fire a raycast from the centre of the mech at the player to see if it hits.
            Vector3 playerDirectionVector = GetDirectionToPlayer();

            // Draw a debug ray so we can see it
            Debug.DrawRay(NPC.transform.position, playerDirectionVector * range, Color.red);

            // Bit shift the index of the layer (8) to get a bit mask
            int layerMask = 1 << 8;

            // This would cast rays only against colliders in layer 8.
            // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
            layerMask = ~layerMask;

            Vector3 startPosition = new Vector3(NPC.transform.position.x, NPC.transform.position.y + sightHeightOffset, NPC.transform.position.z);

            if (Physics.Raycast(startPosition, playerDirectionVector, out RaycastHit hitInfo, range, layerMask))
            {
                //Debug.Log("Hit something");
                //Debug.Log("Hit: " + hitInfo.rigidbody);
                // If the Raycast did hit, then check if it hit the player
                if (hitInfo.transform.CompareTag("Player"))
                {
                    //Debug.Log("Can see player!");
                    return true;
                }
            }
        }

        return false;
    }
}
