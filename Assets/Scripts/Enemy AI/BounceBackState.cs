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

public class BounceBackState : BaseState
{
    public override void EnterState(EnemyFSM npc)
    {
        base.EnterState(npc);

        Debug.Log("Moving away...");

        agent.speed = seekSpeed;
        SetTarget();
    }

    void SetTarget()
    {
        // Get a random point within a defiend sphere around the agent
        Vector3 random_direction = Random.insideUnitSphere * retreatRadius;

        // Get the direction of that point
        random_direction = random_direction + NPC.transform.position;

        // Instantiate our target location
        Vector3 target = Vector3.zero;

        // Get the co-ordinates using a raycast to find the location
        if (NavMesh.SamplePosition(random_direction, out NavMeshHit hit, retreatRadius, 1))
        {
            target = hit.position;
        }

        /*
         * Coroutines must be called from a MonoBehaviour class. Since this is not a monobehaviour,
         * we must instead call the coroutine in this code, but get the MonoBehviour script to run it for us.
         * 
         * Here we are getting the FSM MonoBehaviour to begin this coroutine.
         */
        
        // Set the retreat target for our agent
        FSM.StartCoroutine(MoveAway(target));
    }

    IEnumerator MoveAway(Vector3 target)
    {
        //Debug.Log("Retreating to: " + target);

        // Set retreat location
        agent.destination = target;

        // Wait
        yield return new WaitForSeconds(waitTime);

        // Reset collided tag
        FSM.hit_player = false;

        // Return to seeking the player
        FSM.MoveToState(FSM.seek);
    }
}
