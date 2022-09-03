using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerStateMachine : MonoBehaviour
{
    PlayerState _currentState;

    public static string PLAYER_IDLE_STATE = nameof(PlayerIdleState);
    public static string PLAYER_RUN_STATE = nameof(PlayerRunState);
    public static string PLAYER_STRAFE_STATE = nameof(PlayerStrafeState);
    public static string PLAYER_DEATH_STATE = nameof(PlayerDeathState);

    public Dictionary<string, PlayerState> _states;

    public GameObject PlayerModel;

    public float playerHealth;
    public float originalHealth;

    // Changes needed in order to facilitate player control changes
    //public Vector3 forward => PlayerModel.transform.forward;
    public Vector3 forward;// => Camera.main.transform.forward;

    private List<Material> modelMaterials;
    private float pulseSpeed = 0f;

    [Header("Sounds")]
    public AudioClip audioDamage;
    public AudioClip audioDanger;
    public AudioClip audioDeath;

    public AudioSource audioSourceRegular;
    public AudioSource audioSourceAlarm;

    private bool isAlarmPlaying;
    private bool playerHealthBelowThreshhold;
    private float alarmThreshhold;
    private bool isPlayerDead;


    void Start()
    {
        _states = new Dictionary<string, PlayerState>();
        _states.Add(PLAYER_IDLE_STATE, new PlayerIdleState(this));
        _states.Add(PLAYER_RUN_STATE, new PlayerRunState(this));
        _states.Add(PLAYER_STRAFE_STATE, new PlayerStrafeState(this));
        _states.Add(PLAYER_DEATH_STATE, new PlayerDeathState(this));

        _currentState = _states[PLAYER_IDLE_STATE];

        _currentState.Start(Vector2.zero);

        originalHealth = playerHealth;

        modelMaterials = (PlayerModel.GetComponentsInChildren<SkinnedMeshRenderer>()).Select(x => x.material).ToList();

        // Set the camera-relative direction for movement
        Vector3 cardianlForward = Camera.main.transform.forward;
        cardianlForward.y = 0;
        forward = cardianlForward;
        //Debug.Log(forward);

        isAlarmPlaying = false;
        audioSourceAlarm.clip = audioDanger;
        alarmThreshhold = originalHealth * 0.4f;
        isPlayerDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(_currentState == null)
        {
            Start();
        }

        // Check whether the player's health is low
        playerHealthBelowThreshhold = playerHealth <= alarmThreshhold;

        // If the player is dead, we just want to stop playing this alarm sound. Setting this false should do that. (Bit of a hacky way to do it)
        if (playerHealth <= 0)
        {
            playerHealthBelowThreshhold = false;
            
            // If the player has in fact died, we want to play the death sound
            if (!isPlayerDead)
            {
                // Set this flag to true so that it does not play again
                isPlayerDead = true;

                // Play the death sound
                audioSourceAlarm.clip = audioDeath;
                audioSourceAlarm.Play();
            }
        }
        // If the player's health is low AND the alarm isn't playing, play the alarm
        else if (playerHealthBelowThreshhold && !isAlarmPlaying)
        {
            // Set this flag true so we do not repeat any of this code
            isAlarmPlaying = true;
            // Begin the looping alarm coroutine
            StartCoroutine(PlayAlarm());
        }

        _currentState.Update();
    }

    IEnumerator PlayAlarm()
    {
        // Wait 1 second between each beep
        yield return new WaitForSeconds(1f);

        // Make the alarm noise
        audioSourceAlarm.Play();

        // Check to make sure the player is still below the threshold health
        if (!playerHealthBelowThreshhold)
        {
            // If not, reset the flag and exit the coroutine
            isAlarmPlaying = false;
        }
        else
        {
            // Otherwise, loop back, play the coroutine (And beep) again
            StartCoroutine(PlayAlarm());
        }
    }

    void FixedUpdate()
    {
        _currentState.FixedUpdate();
    }

    public void ChangeState(string stateName, Vector2 input) 
    {
        if (_states.ContainsKey(stateName) && _currentState != _states[stateName])
        {
            _currentState.End();
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
        audioSourceRegular.clip = audioDamage;
        audioSourceRegular.Play();

    }

    private void UpdateHealth(float amount)
    {
        playerHealth += amount;

        if(playerHealth <= 0)
        {
            ChangeState(PLAYER_DEATH_STATE, Vector2.zero);
        }

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
