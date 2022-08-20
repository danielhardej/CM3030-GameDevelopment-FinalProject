﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechSpawnState : MechBaseState
{
     public override void EnterState(MechEnemyFSM npc)
    {
        base.EnterState(npc);

        FSM.StartCoroutine(CheckIsOnGround());
    }

    IEnumerator CheckIsOnGround()
    {
        while (!FSM.isOnGround)
        {
            yield return null;
        }

        Debug.Log("Moving from spawn state");
        //animator.SetBool("IsSleeping", false);
        yield return new WaitForSeconds(1f);
        FSM.MoveToState(FSM.seek);

    }
}
