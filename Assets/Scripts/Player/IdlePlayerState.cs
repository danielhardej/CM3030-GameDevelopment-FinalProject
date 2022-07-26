using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlePlayerState : PlayerState
{

    public IdlePlayerState(PlayerStateMachine stateMachine):base(stateMachine)
    {}

    public override void Start(Vector2 _)
    {
        _animator.SetTrigger("Idle");
    }

    public override void Move(Vector2 input)
    {
        base.Move(input);

        if(input.x != 0f && input.y == 0)
        {
            _stateMachine.ChangeState(nameof(PlayerStrafeState), input);
            return;
        }

        _stateMachine.ChangeState(nameof(RunPlayerState), input);
    }

    public override void End()
    {
        _animator.ResetTrigger("Idle");
    }

}
