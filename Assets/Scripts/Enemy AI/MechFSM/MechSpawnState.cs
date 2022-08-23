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
        if(FSM.isOnGround)
        {
            FSM.MoveToState(FSM.seek);
        }
    }

    public override void Update()
    {
        
    }
}
