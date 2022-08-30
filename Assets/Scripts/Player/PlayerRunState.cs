using System.Collections;
using System;
using UnityEngine;

public class PlayerRunState : PlayerState
{
    float _speed = 10.0f;

    float _acceleration;
    float _strafe;

    public PlayerRunState(PlayerStateMachine stateMachine):base(stateMachine)
    {}

    public override void Start(Vector2 input)
    {
        base.Start(input);

        _animator.SetTrigger("Run");

        CalculateMovementAndRotation(input);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        //var direction = (_stateMachine.forward * _acceleration) + _stateMachine.PlayerModel.transform.right * _strafe;
        var direction = (_stateMachine.forward * _acceleration) + Camera.main.transform.right * _strafe;

        _stateMachine.transform.position += direction * Time.deltaTime;
    }

    //input represents the two input axis the x component is vertical and the y component horizontal.
    //we use horizontal for back and forth and vertical for rotation.
    public override void Move(Vector2 input)
    {
        base.Move(input);

        if(input.sqrMagnitude == 0f)
        {
            _stateMachine.ChangeState(nameof(PlayerIdleState), input);
            return;
        }

        CalculateMovementAndRotation(input);
    }

    public override void End()
    {
        base.End();
        _animator.ResetTrigger("Run");
    }

    private void CalculateMovementAndRotation(Vector2 input)
    {
        _acceleration = input.y * _speed;
        _strafe = (float)Math.Round(input.x)  * _speed/2;
    }

}
