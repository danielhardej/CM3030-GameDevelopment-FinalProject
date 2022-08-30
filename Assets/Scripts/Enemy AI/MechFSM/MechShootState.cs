using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechShootState : MechBaseState
{

    private Vector3 reticlePosition;

    //The time the mech stays in this state in seconds.
    float timeToFire = 2.0f;

    float totalTimeInState;

    float spriteTargetScale;
    float spriteCurrentScale;

    float spriteCurrentAlpha;

    public override void EnterState(MechEnemyFSM npc)
    {
        base.EnterState(npc);

        FSM.animator.SetBool("isWalking", false);

        totalTimeInState = 0f;

        reticlePosition = body.position + body.forward * Vector3.Distance(body.position, player.transform.position);
        reticlePosition.y = 0f;
        spriteTargetScale = FSM.Reticle.transform.localScale.x;
        spriteCurrentScale = 1.0f;
        spriteCurrentAlpha = 0.01f;

        agent.enabled = false;

        FSM.lockRotation = true;
        //FSM.LazerSight.enabled = true;
        FSM.Reticle.enabled = true;
    }

    public override void FixedUpdate()
    {
        if(totalTimeInState >= timeToFire)
        {
            MoveToSeekState();
        }

        if(totalTimeInState >= timeToFire*0.5f)
        {
            var trigger = Mathf.Sin(Mathf.PI * 6 * totalTimeInState);

            if((1 - trigger) < 0.01f)
            {
                FSM.ShootBgCanonA();
            }
            else if ((trigger + 1) < 0.01f)
            {
                FSM.ShootBgCanonB();
            }
            
        }

        totalTimeInState += Time.fixedDeltaTime;

        FSM.SetReticle(reticlePosition);
    }

    public override void Update()
    {
        //FSM.DrawLine(FSM.LazerSight);
        var scaleDelta = Mathf.MoveTowards(spriteCurrentScale, spriteTargetScale, totalTimeInState);
        FSM.Reticle.transform.localScale = new Vector3(scaleDelta, scaleDelta, scaleDelta);
        var colour = FSM.Reticle.material.color;
        colour.a = Mathf.MoveTowards(spriteCurrentAlpha, 1.0f, totalTimeInState);
        FSM.Reticle.material.color = colour;
    }

    private void MoveToSeekState()
    {
        FSM.animator.SetBool("isWalking", true);
        agent.enabled = true;
        FSM.MoveToState(FSM.seek);
        totalTimeInState = 0f;
        FSM.lockRotation = false;
        FSM.LazerSight.enabled = false;
        FSM.Reticle.enabled = false;
    }
}
