using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStrafeState : PlayerState
{
    private const string STRAFE_LEFT = "StrafeLeft";
    private const string STRAFE_RIGHT = "StrafeRight";

    private string _currentTrigger;

    float _speed = 5.0f;

    public PlayerStrafeState(PlayerStateMachine stateMachine): base(stateMachine)
    {}
    // Start is called before the first frame update
    public override void Start(Vector2 input)
    {
        base.Start(input);

        _currentTrigger = input.x > 0 ? STRAFE_LEFT : STRAFE_RIGHT;

        _animator.SetTrigger(_currentTrigger);
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        var direction = _currentTrigger == STRAFE_LEFT ? _stateMachine.PlayerModel.transform.right * (-1f) : _stateMachine.PlayerModel.transform.right;

        _stateMachine.transform.position += direction * _speed * Time.deltaTime;
        Debug.DrawLine(_stateMachine.transform.position, _stateMachine.transform.position + direction * _speed);
    }

    public override void Move(Vector2 input)
    {
        base.Move(input);

        if(input.sqrMagnitude == 0f)
        {
            _stateMachine.ChangeState(nameof(PlayerIdleState), input);
            return;
        }

        if(input.y != 0f)
        {
            _stateMachine.ChangeState(nameof(PlayerRunState), input);
            return;
        }

    }

    public override void End()
    {
        base.End();

        _animator.ResetTrigger(_currentTrigger);
    }

    private void SetAnimationTrigger(string newTrigger)
    {
        _animator.ResetTrigger(_currentTrigger);
        _animator.SetTrigger(newTrigger);

        _currentTrigger = newTrigger;
    }
}
