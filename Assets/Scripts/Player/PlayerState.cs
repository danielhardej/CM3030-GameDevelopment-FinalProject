using UnityEngine;

public abstract class PlayerState
{
    protected PlayerStateMachine _stateMachine;
    protected Animator _animator;

    public PlayerState(PlayerStateMachine stateMachine)
    {
        _stateMachine = stateMachine;

        if(_animator == null)
        {
            _animator = _stateMachine.GetComponentInChildren<Animator>();
        }

    }

    public virtual void Start(){}

    public virtual void Update(){}

    public virtual void FixedUpdate(){}

    public virtual void Move(Vector2 input){}

    public virtual void End(){}
}
