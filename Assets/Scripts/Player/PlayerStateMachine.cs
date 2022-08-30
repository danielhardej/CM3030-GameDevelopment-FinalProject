using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    PlayerState _currentState;

    public Dictionary<string, PlayerState> _states;

    public GameObject PlayerModel;

    public float playerHealth;

    //public Vector3 forward => PlayerModel.transform.forward;
    public Vector3 forward;// => Camera.main.transform.forward;

    void Start()
    {
        _states = new Dictionary<string, PlayerState>();
        _states.Add(nameof(PlayerIdleState), new PlayerIdleState(this));
        _states.Add(nameof(PlayerRunState), new PlayerRunState(this));
        _states.Add(nameof(PlayerStrafeState), new PlayerStrafeState(this));

        _currentState = _states[nameof(PlayerIdleState)];

        _currentState.Start(Vector2.zero);

        Vector3 cardianlForward = Camera.main.transform.forward;
        cardianlForward.y = 0;
        forward = cardianlForward;
        Debug.Log(forward);
    }

    // Update is called once per frame
    void Update()
    {
        if(_currentState == null)
        {
            Start();
        }
       
        _currentState.Update();
    }

    void FixedUpdate()
    {
        _currentState.FixedUpdate();
    }

    public void ChangeState(string stateName, Vector2 input) 
    {
        _currentState.End();

        if (_states.ContainsKey(stateName))
        {
            _currentState = _states[stateName];
            _currentState.Start(input);
        }
        
    }

    public void OnMove(InputValue input)
    {
        Debug.Log("OnMove Called");

        Vector2 inputVec = input.Get<Vector2>();

        _currentState.Move(inputVec);
    }

    /// <summary>
    /// Method <c>ApplyDamage</c> Receives a message sent to this object in order to apply a weapon's damage to this entitiy.
    /// </summary>
    public void ApplyDamage(float damage)
    {
        playerHealth -= damage;

        //Debug.Log("Hit! Took: " + damage + " damage. " + playerHealth + " health remaining");

        //if (health <= 0)
        //{
        //    gameObject.SetActive(false);
        //}
    }
}
