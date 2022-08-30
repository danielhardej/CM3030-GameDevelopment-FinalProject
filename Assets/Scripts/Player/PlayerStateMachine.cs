using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerStateMachine : MonoBehaviour
{
    PlayerState _currentState;

    public Dictionary<string, PlayerState> _states;

    public GameObject PlayerModel;

    public float playerHealth;
    public float originalHealth;
    
    public Vector3 forward => PlayerModel.transform.forward;

    private List<Material> modelMaterials;
    private float pulseSpeed = 0f;

    void Start()
    {
        _states = new Dictionary<string, PlayerState>();
        _states.Add(nameof(PlayerIdleState), new PlayerIdleState(this));
        _states.Add(nameof(PlayerRunState), new PlayerRunState(this));
        _states.Add(nameof(PlayerStrafeState), new PlayerStrafeState(this));

        _currentState = _states[nameof(PlayerIdleState)];

        _currentState.Start(Vector2.zero);

        originalHealth = playerHealth;

        modelMaterials = (PlayerModel.GetComponentsInChildren<SkinnedMeshRenderer>()).Select(x => x.material).ToList();
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

        Vector2 inputVec = input.Get<Vector2>();

        _currentState.Move(inputVec);
    }

    /// <summary>
    /// Method <c>ApplyDamage</c> Receives a message sent to this object in order to apply a weapon's damage to this entitiy.
    /// </summary>
    public void ApplyDamage(float damage)
    {
        UpdateHealth(-damage);
    }

    private void UpdateHealth(float amount)
    {
        playerHealth += amount;

        var healthPercentage = playerHealth / originalHealth;

        GameController.Instance.UpdatePlayerHealth(healthPercentage);

        pulseSpeed = Mathf.MoveTowards(pulseSpeed, 30f, healthPercentage);

        UpdateMatarial(Color.Lerp(Color.red, Color.yellow, healthPercentage), 0.8f,pulseSpeed);

    }

    private void UpdateMatarial(Color color, float amount, float speed)
    {
        foreach (var mat in modelMaterials)
        {
            mat.SetColor("FresnelColour", color);
            mat.SetFloat("FresnelAmount", amount);
            mat.SetFloat("Speed_", speed);
        }
    }

}
