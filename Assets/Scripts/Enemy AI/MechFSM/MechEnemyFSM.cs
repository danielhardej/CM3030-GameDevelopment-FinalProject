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
using System.Linq;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class MechEnemyFSM : MonoBehaviour
{
    #region Variables
    public MechBaseState currentState;

    [Header("States")]
    public MechSeekState seek = new MechSeekState();
    public MechWalkShootState walkAndShoot = new MechWalkShootState();
    public MechSpawnState spawnState = new MechSpawnState();

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
    public GameObject AimingPoint;
    public float mainGunFiringRate;
    public float mainGunDamage;
    [HideInInspector]
    public bool isFiringMain;
    [HideInInspector]
    public bool mainLR;

    public float secondaryGunFiringRate;
    public float secondaryGunDamage;
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

    [HideInInspector]
    public NavMeshAgent agent;

    [HideInInspector]
    public GameObject player;

    [HideInInspector]
    public GameObject NPCgO; //NPC Game Object

    [HideInInspector]
    public Transform body;

    public bool isOnGround = false;

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

        AimingPoint = GameObject.FindGameObjectsWithTag("Player").First().transform.Find("AimPoint").gameObject;

        MoveToState(spawnState);
    }

    void Update()
    {
        currentState.Update();

        DrawLine(BigCanon01L, 2.75f);
        DrawLine(BigCanon01R, 2.75f);
        DrawLine(BigCanon02L, 2.75f);
        DrawLine(BigCanon02R, 2.75f);

        DrawLine(SmallCanon01L, 1.25f);
        DrawLine(SmallCanon01R, 1.25f);
        DrawLine(SmallCanon02L, 1.25f);
        DrawLine(SmallCanon02R, 1.25f);
    }

    void FixedUpdate()
    {
        currentState.FixedUpdate();

        if(!agent.enabled)
        {
            isOnGround = Physics.Raycast(transform.position + Vector3.up, Vector3.down, 1f);

            if(isOnGround)
            {
                agent.enabled = true;
            }
        }
        
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

        Debug.Log("Hit! Took: " + damage + " damage. " + health + " health remaining");
        GameController.Instance.IncreaseScore(10);

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
        if(AimingPoint == null) return;

        Vector3 offset = lr.transform.right * -forwardOffset;

        Vector3 start = lr.transform.position + offset;
        lr.SetPosition(0, start);
        lr.SetPosition(1, AimingPoint.transform.position);
    }

    #region Cannon stuff
    RaycastHit FireAtTarget(Vector3 from, GameObject target, float damage)
    {
        if(target == null) return new RaycastHit();
        // Fire a raycast from the centre of the mech at the player to see if it hits.
        Vector3 playerDirectionVector = (target.transform.position - from).normalized;

        // Draw a debug ray so we can see it
        Debug.DrawRay(from, playerDirectionVector * Vector3.Distance(from, target.transform.position), Color.cyan);

        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        //Checks if we hit something
        if (Physics.Raycast(from, playerDirectionVector, out RaycastHit hitInfo, range, layerMask))
        {
            // If we did, apply necessary damage!
            hitInfo.rigidbody.SendMessage("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
        }

        // Return this information in case it's useful
        return hitInfo;
    }

    //Big Canons
    public void ShootBigCanonA()
    {
        Debug.Log("ShootBigCanonA");
        animator.SetTrigger("ShootBigCanonA");
        FireAtTarget(BigCanon01L.transform.position, AimingPoint, mainGunDamage);
        FireAtTarget(BigCanon01R.transform.position, AimingPoint, mainGunDamage);

        audioSource.clip = audioBigCanon;
        audioSource.Play();

        Color c = BigCanon01L.material.GetColor("_TintColor");
        c.a = 1f;
        BigCanon01L.material.SetColor("_TintColor", c);
        BigCanon01R.material.SetColor("_TintColor", c);

        StartCoroutine(FadoutCanon(BigCanon01L, BigCanon01R));
    }


    public void ShootBigCanonB()
    {
        animator.SetTrigger("ShootBigCanonB");
        FireAtTarget(BigCanon02L.transform.position, AimingPoint, mainGunDamage);
        FireAtTarget(BigCanon02R.transform.position, AimingPoint, mainGunDamage);

        audioSource.clip = audioBigCanon;
        audioSource.Play();

        Color c = BigCanon01L.material.GetColor("_TintColor");
        c.a = 1f;
        BigCanon02L.material.SetColor("_TintColor", c);
        BigCanon02R.material.SetColor("_TintColor", c);
        StartCoroutine(FadoutCanon(BigCanon02L, BigCanon02R));
    }


    // Small Canons
    public void ShootSmallCanonA()
    {
        animator.SetTrigger("ShootSmallCanonA");
        FireAtTarget(SmallCanon01L.transform.position, AimingPoint, mainGunDamage);
        FireAtTarget(SmallCanon01R.transform.position, AimingPoint, mainGunDamage);

        audioSource.clip = audioSmallCanon;
        audioSource.Play();

        Color c = SmallCanon01L.material.GetColor("_TintColor");
        c.a = 1f;
        SmallCanon01L.material.SetColor("_TintColor", c);
        SmallCanon01R.material.SetColor("_TintColor", c);
        StartCoroutine(FadoutCanon(SmallCanon01L, SmallCanon01R));
    }

    public void ShootSmallCanonB()
    {
        animator.SetTrigger("ShootSmallCanonB");
        FireAtTarget(SmallCanon02L.transform.position, AimingPoint, mainGunDamage);
        FireAtTarget(SmallCanon02R.transform.position, AimingPoint, mainGunDamage);

        audioSource.clip = audioSmallCanon;
        audioSource.Play();

        Color c = SmallCanon01L.material.GetColor("_TintColor");
        c.a = 1f;
        SmallCanon02L.material.SetColor("_TintColor", c);
        SmallCanon02R.material.SetColor("_TintColor", c);
        StartCoroutine(FadoutCanon(SmallCanon02L, SmallCanon02R));
    }

    IEnumerator FadoutCanon(LineRenderer CanonL, LineRenderer CanonR)
    {
        Color c = CanonL.material.GetColor("_TintColor");
        while (c.a > 0)
        {
            c.a -= 0.1f;
            CanonL.material.SetColor("_TintColor", c);
            CanonR.material.SetColor("_TintColor", c);
            yield return null;
        }
    }
    #endregion
}
