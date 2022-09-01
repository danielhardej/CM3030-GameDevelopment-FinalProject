using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerState
{
    public PlayerDeathState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Start(Vector2 input)
    {
        base.Start(input);
        _animator.SetTrigger("Die");
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void End()
    {
        base.End();
        _animator.ResetTrigger("Die");
    }

}
