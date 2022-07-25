using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunPlayerState : PlayerState
{

    float _speed = 5.0f;
    float _rotationSpeed = 5.0f;

    Vector3 _movement;
    Quaternion _rotation;

    public RunPlayerState(PlayerStateMachine stateMachine):base(stateMachine)
    {}

    public override void Start()
    {
        base.Start();

        _animator.SetTrigger("Run");
        _movement = new Vector3(0,0,_speed);     
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        _stateMachine.transform.position += _movement * Time.deltaTime;
        //_stateMachine.transform.rotation += _rotation * new Vector3(Time.deltaTime,Time.deltaTime,Time.deltaTime);

    }

    //input represents a the two input axis the x component is vertical and the y component horizontal.
    //we use horizontal for back and forth and vertical for rotation.
    public override void Move(Vector2 input)
    {
        base.Move(input);

        if(input.sqrMagnitude == 0f)
        {
            _stateMachine.ChangeState(nameof(IdlePlayerState));
            return;
        }

        _movement = new Vector3(0,0, input.y * _speed);
        _rotation = Quaternion.AngleAxis(input.x * _rotationSpeed, Vector3.up);
        Debug.Log(_movement);
    }

    public override void End()
    {
        base.End();
        _animator.ResetTrigger("Run");
    }

}
