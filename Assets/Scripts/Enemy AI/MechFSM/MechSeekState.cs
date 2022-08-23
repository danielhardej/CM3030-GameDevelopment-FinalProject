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

/// <summary>
/// State <c>MechSeekState</c> Is a behavioural state for the mech enemy.
/// <para>
/// <c>MechSeekState</c> has the mech set its target location to that of the player.
/// It will path to and approach the player, checking if it has line of sight.
/// If the mech gains line of sight, it moves into the <c>MechWalkShootState</c>.
/// </para>
/// </summary>
public class MechSeekState : MechBaseState
{
    float timeSinceStateEntered;
    public override void EnterState(MechEnemyFSM npc)
    {
        base.EnterState(npc);

        // Triggers the start of the walking animation
        animator.SetBool("isWalking", true);

        timeSinceStateEntered = 0f;
    }

    public override void FixedUpdate()
    {
        timeSinceStateEntered += Time.fixedDeltaTime;

        // Waits the prescribed amount of time
        if(timeSinceStateEntered > targetPositionUpdateTime)
        {
            // If we have line of sight with the player, then we begin shooting!
            if(HasLineOfSight())
            {
                FSM.MoveToState(FSM.walkAndShoot);
            }
            else // Otherwise, we continue moving toward the player
            {
                destination = player.transform.position;
                agent.SetDestination(destination);
            }
            // Re-set the timer
            timeSinceStateEntered = 0f;
        }
    }

    public override void Update()
    {}
}
