using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechSpawnState : MechBaseState
{
    public override void EnterState(MechEnemyFSM npc)
    {
        base.EnterState(npc);
    }

    public override void FixedUpdate()
    {
        if(!agent.enabled)
        {
            var isOnGround = Physics.Raycast(FSM.transform.position + Vector3.up, Vector3.down, heightCheck);

            if(isOnGround)
            {
                agent.enabled = true;
                FSM.MoveToState(FSM.seek);
            }
        }
    }

    public override void Update()
    {
        
    }
}
