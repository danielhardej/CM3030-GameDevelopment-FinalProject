using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlePlayerState : PlayerState
{

    public IdlePlayerState(PlayerStateMachine stateMachine):base(stateMachine)
    {}

    public override void Start()
    {
        _animator.SetTrigger("Idle");
    }

    public override void Move(Vector2 input)
    {
        base.Move(input);
        _stateMachine.ChangeState(nameof(RunPlayerState));
    }

    public override void End()
    {
        _animator.ResetTrigger("Idle");
    }

}
