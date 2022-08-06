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

public class MechWalkShootState : MechBaseState
{
    public override void EnterState(MechEnemyFSM npc)
    {
        base.EnterState(npc);

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
        // Waits the presribed amount of time
        yield return new WaitForSeconds(targetPositionUpdateTime);

        // If the player is within shooting range
        if (Vector3.Distance(NPC.transform.position, player.transform.position) <= preferredRange)
        {
            // Fire a raycast from the centre of the mech at the player to see if it hits.
            Vector3 player_direction_vector = (NPC.transform.position - player.transform.position).normalized;
            if (Physics.Raycast(NPC.transform.position, player_direction_vector, out RaycastHit hitInfo, range))
            {
                // If the Raycast did hit, then check if it hit the player
                if (hitInfo.transform.CompareTag("Player"))
                {
                    // If all of this is true, Move to the next state
                    FSM.MoveToState(FSM.walkAndShoot);

                    // Stop the agent
                    agent.isStopped = true;

                    // This breaks us out of the IEnumerator and does not run the code below.
                    yield break;
                }
            }
        }
        // If any of the statements above were false, this runs which re-runs this IEnumerator (Like a loop!)
        //Debug.Log("Setting target to:" + player.transform.position);
        agent.destination = player.transform.position;
        FSM.StartCoroutine(SeekStatusCheck());
    }
}
