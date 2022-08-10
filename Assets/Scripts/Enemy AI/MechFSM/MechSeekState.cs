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

public class MechSeekState : MechBaseState
{
    public override void EnterState(MechEnemyFSM npc)
    {
        base.EnterState(npc);
        Debug.Log("Entering Seek State");

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
        // Waits the presribed amount of time
        yield return new WaitForSeconds(targetPositionUpdateTime);

        float distanceToPlayer = Vector3.Distance(NPC.transform.position, player.transform.position);
        //Debug.Log("Distance: " + distanceToPlayer);

        // If the player is within shooting range
        if (distanceToPlayer <= range)
        {
            Debug.Log("Within range");
            // Fire a raycast from the centre of the mech at the player to see if it hits.
            Vector3 playerDirectionVector = (player.transform.position - NPC.transform.position).normalized;
            Debug.DrawRay(NPC.transform.position, playerDirectionVector * range, Color.red);

            // Bit shift the index of the layer (8) to get a bit mask
            int layerMask = 1 << 8;

            // This would cast rays only against colliders in layer 8.
            // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
            layerMask = ~layerMask;

            if (Physics.Raycast(NPC.transform.position, playerDirectionVector, out RaycastHit hitInfo, range, layerMask))
            {
                Debug.Log("Hit something");
                Debug.Log("Hit: " + hitInfo.rigidbody);
                // If the Raycast did hit, then check if it hit the player
                if(hitInfo.transform.CompareTag("Player"))
                {
                    Debug.Log("Can see player!");
                    // If all of this is true, Move to the next state
                    FSM.MoveToState(FSM.walkAndShoot);

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
