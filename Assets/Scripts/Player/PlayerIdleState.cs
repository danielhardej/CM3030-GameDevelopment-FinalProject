using UnityEngine;

public class PlayerIdleState : PlayerState
{

    public PlayerIdleState(PlayerStateMachine stateMachine):base(stateMachine)
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

        _stateMachine.ChangeState(nameof(PlayerRunState), input);
    }

    public override void End()
    {
        _animator.ResetTrigger("Idle");
    }

}
