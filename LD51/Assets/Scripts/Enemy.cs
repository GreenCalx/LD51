using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Tweaks")]
    public float timeBeforeDeleting = 60f;
    public float sinkSpeed = 1f;

    [Header("Gamestates")]
    public bool isDead = false;

    private Rigidbody rb;

    [SerializeField]
    private bool canSeePlayer = false;

    ///
    private float elapsedTimeBeforeDeletion = 0f;
    public Transform    impalingObject;
    public Vector3      impalingLocalisation;
    public Vector3      impalingNormal;

    public float swimLatch;
    private float currentSwitchLatch;

    public float timerRndDirChg;
    public float currentTimerRndDirChg;

public BoxCollider bc;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (!bc)
            bc = GetComponent<BoxCollider>();

        patrol = bc != null;

        isDead = false;
        elapsedTimeBeforeDeletion = 0f;
        
        impalingObject = null;
        impalingLocalisation = Vector3.zero;
        impalingNormal = Vector3.zero;

        currentSwitchLatch = swimLatch;
    }

    public bool canSwim = false;
    public bool patrol = false;

    // Update is called once per frame
    void Update()
    {
        currentSwitchLatch -= Time.deltaTime;
        currentTimerRndDirChg += Time.deltaTime;
        if(currentSwitchLatch <= 0f) {
            currentSwitchLatch = swimLatch;
            canSwim = !canSwim;
        }


        if (isDead)
        {
            elapsedTimeBeforeDeletion += Time.deltaTime;
            if (elapsedTimeBeforeDeletion > timeBeforeDeleting)
            {
                Destroy(gameObject);
            }

            // plunge
            //rb.AddForce(-Vector3.up * sinkSpeed, ForceMode.Force);

            return;
        }

        // ..

        if (canSeePlayer) AttackPlayer();
        else if (patrol) RandomMove();
    }

    public void impale(Transform impaler, Vector3 localisation, Vector3 normal)
    {
        Debug.Log("IMPALED!!!!!!");
        impaler.position = localisation;
        impaler.rotation = Quaternion.FromToRotation(Vector3.up, normal); 
        impalingObject = impaler;
        impalingLocalisation = localisation;
        impalingNormal = normal;
        isDead = true;
        rb.drag = 0.2f;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag==Constants.TAG_GROTTO)
        {
            rb.isKinematic = true;
        }
    }

    void OnTriggerEnter(Collider C) {
        if (C.GetComponent<PlayerController>())
            canSeePlayer = true;
    }
    void OnTriggerExit(Collider C) {
        if (C.GetComponent<PlayerController>()) {
            canSeePlayer = false;
        };
    }

    void GoTowards(Vector3 desiredPosition) {
       targetPosition = desiredPosition;
       
        if (WallReached()) {
            targetPosition = bc.bounds.center;
            rb.velocity = Vector3.zero;
        }

        // orient forward towards target
        var newForward = desiredPosition - transform.position;
        var newRight = Vector3.Cross(transform.up, newForward);
        var newUp = Vector3.Cross(newForward, newRight);
        var rotation = Quaternion.LookRotation(newForward, transform.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 90*Time.deltaTime);

       if (PathIsFree()) {
            transform.rotation = Quaternion.AngleAxis( 90 * Time.deltaTime * Mathf.Cos(Time.realtimeSinceStartup), transform.forward ) * transform.rotation;
            if (canSwim) {
                transform.position += transform.forward * 3f * Time.deltaTime;
            }
       }
    }

    public float playerBoundRadius = 2f;
    void AttackPlayer() {
       GoTowards(Access.Player().transform.position);
    }

    public Vector3 targetPosition;
    bool TargetReached() {
        return (targetPosition - transform.position).magnitude < playerBoundRadius ;
    }

    bool PathIsFree() {
        return !Physics.Raycast(transform.position, transform.forward * 5f, 3f);
    }

    bool WallReached() {
        return bc ? !bc.bounds.Contains(rb.position) : false; 
    }
    void RandomMove() {
        if (currentTimerRndDirChg >= timerRndDirChg || (TargetReached())) {
            // chose random pos in boundaries
            var newDir = Random.insideUnitSphere * GetComponent<SphereCollider>().radius;
            var newDesiredTarget = transform.position + newDir;
            GoTowards(newDesiredTarget);
            currentTimerRndDirChg = 0;
         } else {
             GoTowards(targetPosition);
         }
    }
}
