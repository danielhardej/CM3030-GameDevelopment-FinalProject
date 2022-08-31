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
/// State <c>MechWalkShootState</c> Is a behavioural state for the mech enemy.
/// <para>
/// <c>MechWalkShootState</c> has the mech walk around the player, shooting at them.
/// 
/// </para>
/// </summary>
public class MechWalkShootState : MechBaseState
{
    float timeSinceStateEntered;
    float timeSinceMainGunsFired;
    float timeSinceSecondaryGunsFired;
    Queue<Vector3> playerPositions;

    Vector3 currentReticle;

    public override void EnterState(MechEnemyFSM npc)
    {
        base.EnterState(npc);

        timeSinceStateEntered = 0f;
        timeSinceMainGunsFired = mainGunFiringRate;
        timeSinceSecondaryGunsFired = secondaryGunFiringRate;

        FSM.LazerSight.enabled = true;
        FSM.Reticle.enabled = true;

        playerPositions = new Queue<Vector3>();

        for (int i = 0; i < Mathf.FloorToInt(0.5f/Time.fixedDeltaTime); i++)
        {
            playerPositions.Enqueue(FSM.AimingPoint.transform.position);
        }

        currentReticle = playerPositions.Dequeue();

        // Set a new destination for the navmeshagent
        SetDestination();

        /*
         * Coroutines must be called from a MonoBehaviour class. Since this is not a monobehaviour,
         * we must instead call the coroutine in this code, but get the MonoBehviour script to run it for us.
         * 
         * Here we are getting the FSM MonoBehaviour to begin this coroutine.
         */
        //FSM.StartCoroutine(SeekStatusCheck());
    }

    public override void Update()
    {
        FSM.DrawLine(FSM.LazerSight);

        Debug.DrawRay(new Vector3(NPC.transform.position.x, NPC.transform.position.y + sightHeightOffset, NPC.transform.position.z), GetDirectionToPlayer() * range, Color.yellow);
    }

    public override void FixedUpdate()
    {
        timeSinceStateEntered += Time.fixedDeltaTime;

        var historicalPlayerPos = playerPositions.Dequeue();

        var playerPositionDistance = Vector3.Distance(historicalPlayerPos, FSM.body.position);

        var activeTargetPosition = FSM.body.position + FSM.body.forward * playerPositionDistance;

        activeTargetPosition.y = historicalPlayerPos.y;

        // Waits the prescribed amount of time
        if(timeSinceStateEntered > targetPositionUpdateTime)
        {
            // If we have lost line of sight with the player, go back to seeking.
            if (Vector3.Distance(activeTargetPosition, NPC.transform.position) > range)
            {
                // Move back to the seek state
                FSM.MoveToState(FSM.seek);
                FSM.LazerSight.enabled = false;
                FSM.Reticle.enabled = false;
                playerPositions.Clear();
            }
            // If we are close to our destination, select a new one
            else if (Vector3.Distance(activeTargetPosition, destination) < 5)
            {
                SetDestination();
            }
            
            // Re-set the timer
            timeSinceStateEntered = 0f;
        }

        //FireMainCannons();
        //FireSecondaryCannons();

        //update the players position
        if(playerPositions.Count == 0)
        {
            playerPositions.Enqueue(FSM.AimingPoint.transform.position);
            playerPositions.Enqueue(FSM.AimingPoint.transform.position);    
        }

        playerPositions.Enqueue(FSM.AimingPoint.transform.position);

        FSM.SetReticle(activeTargetPosition);

    }

    IEnumerator SeekStatusCheck()
    {
        // Waits the prescribed amount of time
        yield return new WaitForSeconds(targetPositionUpdateTime);

        FireMainCannons();
        FireSecondaryCannons();

        //Debug.Log(Vector3.Distance(NPC.transform.position, destination));

        // If we have lost line of sight with the player, go back to seeking.
        if (!HasLineOfSight())
        {
            // Move back to the seek state
            FSM.MoveToState(FSM.seek);
            // Stop the execution of this coroutine
            yield break;
        }
        // If we are close to our destination, select a new one
        else if (Vector3.Distance(NPC.transform.position, destination) < 5)
        {
            SetDestination();
        }
        // Otherwise, continue this state
        yield return SeekStatusCheck();
    }

    void FireMainCannons()
    {
        //isFiringMain = timeSinceMainGunsFired > 0;

        if (!isFiringMain) {
            if(mainLR)
            {
                FSM.ShootBgCanonA();
            }
            else
            {
                FSM.ShootBgCanonB();
            }
            mainLR = !mainLR;
            
            isFiringMain = true;
            FSM.StartCoroutine(MainCooldown());
            timeSinceMainGunsFired = mainGunFiringRate;
        }

        timeSinceMainGunsFired -= Time.fixedDeltaTime;
    }

    IEnumerator MainCooldown()
    {
        // Waits the prescribed amount of time
        yield return new WaitForSeconds(mainGunFiringRate);

        isFiringMain = false;
    }

    void FireSecondaryCannons()
    {
        if (!isFiringSecondary)
        {
            if (secondLR)
            {
                FSM.ShootSmllCanonA();
            }
            else
            {
                FSM.ShootSmllCanonB();
            }
            secondLR = !secondLR;

            isFiringSecondary = true;
            FSM.StartCoroutine(SecondaryCooldown());
        }
    }

    IEnumerator SecondaryCooldown()
    {
        // Waits the prescribed amount of time
        yield return new WaitForSeconds(secondaryGunFiringRate);

        isFiringSecondary = false;
    }

    /// <summary>
    /// Method <c>RotateTorso</c> Rotates the mech's torso to face the player.
    /// </summary>
    private void RotateTorso()
    {
        // This is currently being handled in the FSM
    }

    /// <summary>
    /// Method <c>FireAtPlayer</c> Fires the guns at the player
    /// </summary>
    private void FireAtPlayer()
    {
        
    }

    /// <summary>
    /// Method <c>SetDestination</c> Resets the NavMeshAgent's current destination target to a new target centred around the player
    /// </summary>
    private void SetDestination()
    {
        Vector3 newDestination = GetRandomLocation(playerPositions.Peek(), preferredRange);

        destination = newDestination;
        agent.SetDestination(destination);
    }

    /// <summary>
    /// Method <c>GetRandomLocation</c> Gets a random location on the navmesh, centred around <c>centre</c> with radius <c>radius</c>
    /// </summary>
    private Vector3 GetRandomLocation(Vector3 centre, float radius)
    {
        // Get a random point within a defiend sphere around the agent
        Vector3 random_direction = Random.insideUnitSphere * radius;

        // Get the direction of that point
        random_direction += centre;

        // Instantiate our target location
        // We instatiate in at the player's location in case a point isn't found in the next step
        Vector3 target = player.transform.position;

        // Get the co-ordinates using a raycast to find the location
        if (NavMesh.SamplePosition(random_direction, out NavMeshHit hit, radius, 1))
        {
            target = hit.position;
        }

        return target;
    }
}
