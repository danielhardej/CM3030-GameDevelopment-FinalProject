using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    PlayerState _currentState;

    public Dictionary<string, PlayerState> _states;

    void Start()
    {
        _states = new Dictionary<string, PlayerState>();
        _states.Add(nameof(IdlePlayerState), new IdlePlayerState(this));
        _states.Add(nameof(RunPlayerState), new RunPlayerState(this));

        _currentState = _states[nameof(IdlePlayerState)];

        _currentState.Start();

    }

    // Update is called once per frame
    void Update()
    {
        _currentState.Update();
    }

    void FixedUpdate()
    {
        _currentState.FixedUpdate();
    }

    public void ChangeState(string stateName) 
    {
        _currentState.End();

        if (_states.ContainsKey(stateName))
        {
            _currentState = _states[stateName];
            _currentState.Start();
        }
        
    }

    public void OnMove(InputValue input)
    {
        Debug.Log("OnMove Called");

        Vector2 inputVec = input.Get<Vector2>();

        _currentState.Move(inputVec);
    }
}
