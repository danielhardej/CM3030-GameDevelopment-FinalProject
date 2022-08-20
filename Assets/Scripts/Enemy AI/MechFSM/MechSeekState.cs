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
    public override void EnterState(MechEnemyFSM npc)
    {
        base.EnterState(npc);

        // Triggers the start of the walking animation
        animator.SetBool("isWalking", true);

        /*
         * Coroutines must be called from a MonoBehaviour class. Since this is not a monobehaviour,
         * we must instead call the coroutine in this code, but get the MonoBehviour script to run it for us.
         * 
         * Here we are getting the FSM MonoBehaviour to begin this coroutine.
         */
        FSM.StartCoroutine(SeekStatusCheck());
    }

    IEnumerator SeekStatusCheck()
    {
        // Waits the prescribed amount of time
        yield return new WaitForSeconds(targetPositionUpdateTime);

        // If we have line of sight with the player, then we begin shooting!
        if(HasLineOfSight())
        {
            FSM.MoveToState(FSM.walkAndShoot);
        }
        // Otherwise, we continue moving toward the player
        else
        {
            //Debug.Log("Setting target to:" + player.transform.position);
            destination = player.transform.position;
            agent.destination = destination;

            // Re-run the coroutine
            FSM.StartCoroutine(SeekStatusCheck());
        }
    }
}
