/*
 * 
 * Code by: 
 *      Dimitrios Vlachos
 *      djv1@student.london.ac.uk
 *      dimitri.j.vlachos@gmail.com
 *      
 * Adapted from our FSM lecture
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class MechEnemyFSM : MonoBehaviour
{
    #region Variables
    public MechBaseState currentState;

    [Header("States")]
    public MechSeekState seek = new MechSeekState();
    public MechWalkShootState walkAndShoot = new MechWalkShootState();

    [Header("Animation")]
    public LineRenderer BigCanon01L;
    public LineRenderer BigCanon01R;
    public LineRenderer BigCanon02L;
    public LineRenderer BigCanon02R;
    public LineRenderer SmallCanon01L;
    public LineRenderer SmallCanon01R;
    public LineRenderer SmallCanon02L;
    public LineRenderer SmallCanon02R;
    [HideInInspector]
    public Animator animator;

    [Header("Guns")]
    public float mainGunFiringRate;
    [HideInInspector]
    public bool isFiringMain;
    [HideInInspector]
    public bool mainLR;

    public float secondaryGunFiringRate;
    [HideInInspector]
    public bool isFiringSecondary;
    [HideInInspector]
    public bool secondLR;

    [Header("Sounds")]
    public AudioClip audioBigCanon;
    public AudioClip audioSmallCanon;
    [HideInInspector]
    AudioSource audioSource;

    [Header("NPC Settings")]
    [Tooltip("Maximum health")]
    public float health;
    [Tooltip("Maximum range of weapons")]
    public float range;
    [Tooltip("The distance the mech will attempt to approach to while firing")]
    public float preferredRange;

    [Header("Movement")]
    [Tooltip("The time, in seconds, between target position updates")]
    public float updateTime;
    [Tooltip("Vertical offset of sight-line, used for shooting")]
    public float sightHeightOffset;
    [Tooltip("The mech's target location for it's NavMeshAgent"), ReadOnly]
    public Vector3 destination;

    //[Header("Targeting")]
    //[Tooltip("Left/Right rotation angle toward target"), ReadOnly]
    //public float angleToTarget;
    //[Tooltip("Up/Down rotation angle toward target"), ReadOnly]
    //public float targetDepression;

    [HideInInspector]
    public NavMeshAgent agent;

    [HideInInspector]
    public GameObject player;

    [HideInInspector]
    public GameObject NPCgO; //NPC Game Object

    [HideInInspector]
    public Transform body;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        isFiringMain = false;
        mainLR = false;
        isFiringSecondary = false;
        secondLR = false;

        audioSource = GetComponent<AudioSource>();

        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        NPCgO = this.gameObject;

        body = transform.Find("Mech/Root/Pelvis/Body");

        MoveToState(seek);
    }

    void Update()
    {
        DrawLine(BigCanon01L, 2.75f);
        DrawLine(BigCanon01R, 2.75f);
        DrawLine(BigCanon02L, 2.75f);
        DrawLine(BigCanon02R, 2.75f);

        DrawLine(SmallCanon01L, 1.25f);
        DrawLine(SmallCanon01R, 1.25f);
        DrawLine(SmallCanon02L, 1.25f);
        DrawLine(SmallCanon02R, 1.25f);
    }

    void LateUpdate()
    {
        TurnToFaceTarget(player);
    }

    public void MoveToState(MechBaseState state)
    {
        Debug.Log("Entering state: " + state);
        currentState = state;
        currentState.EnterState(this);
    }

    /// <summary>
    /// Method <c>ApplyDamage</c> Receives a message sent to this object in order to apply a weapon's damage to this entitiy.
    /// </summary>
    public void ApplyDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Method <c>TurnToFaceTarget</c> Turns the mech's torso to face the target
    /// </summary>
    private void TurnToFaceTarget(GameObject target)
    {
        // Determine which direction to rotate towards
        Vector3 targetDirection = target.transform.position - transform.position;

        // The step size is equal to speed times frame time.
        float singleStep = 100 * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(body.forward, targetDirection, singleStep, 0.0f);

        // Draw a ray pointing at our target in
        Debug.DrawRay(transform.position, newDirection* Vector3.Distance(transform.position, player.transform.position), Color.yellow);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        Quaternion rotation = Quaternion.LookRotation(newDirection, transform.forward);

        Quaternion q = rotation;
        q.eulerAngles = new Vector3(q.eulerAngles.x, q.eulerAngles.y, -90);

        body.rotation = q;
    }

    /// <summary>
    /// Method <c>DrawLine</c> Sets the positions of the linerenderer passed through to draw between the player and the guns.
    /// </summary>
    private void DrawLine(LineRenderer lr, float forwardOffset = 0)
    {
        Vector3 offset = lr.transform.right * -forwardOffset;

        Vector3 start = lr.transform.position + offset;
        lr.SetPosition(0, start);
        lr.SetPosition(1, player.transform.position);
    }

    #region Cannon stuff
    //Big Canons
    public void ShootBigCanonA()
    {

        audioSource.clip = audioBigCanon;
        audioSource.Play();

        Color c = BigCanon01L.material.GetColor("_TintColor");
        c.a = 1f;
        BigCanon01L.material.SetColor("_TintColor", c);
        BigCanon01R.material.SetColor("_TintColor", c);

        StartCoroutine("FadoutBigCanon01");
    }

    IEnumerator FadoutBigCanon01()
    {
        Color c = BigCanon01L.material.GetColor("_TintColor");
        while (c.a > 0)
        {
            c.a -= 0.1f;
            BigCanon01L.material.SetColor("_TintColor", c);
            BigCanon01R.material.SetColor("_TintColor", c);
            yield return null;
        }
    }

    public void ShootBigCanonB()
    {

        audioSource.clip = audioBigCanon;
        audioSource.Play();

        Color c = BigCanon01L.material.GetColor("_TintColor");
        c.a = 1f;
        BigCanon02L.material.SetColor("_TintColor", c);
        BigCanon02R.material.SetColor("_TintColor", c);
        StartCoroutine("FadoutBigCanon02");
    }

    IEnumerator FadoutBigCanon02()
    {
        Color c = BigCanon02L.material.GetColor("_TintColor");
        while (c.a > 0)
        {
            c.a -= 0.1f;
            BigCanon02L.material.SetColor("_TintColor", c);
            BigCanon02R.material.SetColor("_TintColor", c);
            yield return null;
        }
    }


    // Small Canons
    public void ShootSmallCanonA()
    {

        audioSource.clip = audioSmallCanon;
        audioSource.Play();

        Color c = SmallCanon01L.material.GetColor("_TintColor");
        c.a = 1f;
        SmallCanon01L.material.SetColor("_TintColor", c);
        SmallCanon01R.material.SetColor("_TintColor", c);
        StartCoroutine("FadoutSmallCanon01");
    }

    IEnumerator FadoutSmallCanon01()
    {
        Color c = SmallCanon01L.material.GetColor("_TintColor");
        while (c.a > 0)
        {
            c.a -= 0.1f;
            SmallCanon01L.material.SetColor("_TintColor", c);
            SmallCanon01R.material.SetColor("_TintColor", c);
            yield return null;
        }
    }

    public void ShootSmallCanonB()
    {

        audioSource.clip = audioSmallCanon;
        audioSource.Play();

        Color c = SmallCanon01L.material.GetColor("_TintColor");
        c.a = 1f;
        SmallCanon02L.material.SetColor("_TintColor", c);
        SmallCanon02R.material.SetColor("_TintColor", c);
        StartCoroutine("FadoutSmallCanon02");
    }

    IEnumerator FadoutSmallCanon02()
    {
        Color c = SmallCanon02L.material.GetColor("_TintColor");
        while (c.a > 0)
        {
            c.a -= 0.1f;
            SmallCanon02L.material.SetColor("_TintColor", c);
            SmallCanon02R.material.SetColor("_TintColor", c);
            yield return null;
        }
    }
    #endregion
}
