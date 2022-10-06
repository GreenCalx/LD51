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
    public Transform impalingObject;
    public Vector3 impalingLocalisation;
    public Vector3 impalingNormal;

    public float swimLatch;
    private float currentSwitchLatch;

    public float timerRndDirChg;
    public float currentTimerRndDirChg;

    public BoxCollider bc;

    public LayerMask mask;

    public enum Type {
        shark,
        medusa
    };
    public Type type;

    public bool killonsight;

    private PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        patrol = bc != null;

        isDead = false;
        elapsedTimeBeforeDeletion = 0f;

        impalingObject = null;
        impalingLocalisation = Vector3.zero;
        impalingNormal = Vector3.zero;

        currentSwitchLatch = swimLatch;

        for (int i = 0; i < 32; i++)
        {
            if (!Physics.GetIgnoreLayerCollision(gameObject.layer, i)) mask |= (1 << i);
        }

    }

    public bool canSwim = false;
    public bool patrol = false;

    // Update is called once per frame
    void Update()
    {
        currentSwitchLatch -= Time.deltaTime;
        currentTimerRndDirChg += Time.deltaTime;
        if (currentSwitchLatch <= 0f)
        {
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
        rb.isKinematic = true;
        //Destroy(gameObject);
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("collision ennemy");
        if (other.transform.tag == Constants.TAG_GROTTO)
        {
            //rb.isKinematic = true;
        }
    }

    void OnTriggerEnter(Collider C)
    {
        Debug.Log("ennemy trigger enter : " + gameObject.name + "  " + C.gameObject.name);
        if (C.gameObject.name == "player") {
            canSeePlayer = true;
            player = C.gameObject.GetComponentInParent<PlayerController>();

        if (killonsight)
            C.gameObject.GetComponentInParent<PlayerController>().kill();
        }

    }
    void OnTriggerExit(Collider C)
    {
        //Debug.Log("ennemy trigger exit : " + gameObject.name + "  " + C.gameObject.name);
        //if (C.gameObject.name == "player")
        //    canSeePlayer = false;
    }

    void GoTowards(Vector3 desiredPosition)
    {
        targetPosition = desiredPosition;

        if (WallReached())
        {
            targetPosition = bc.bounds.center;
            rb.velocity = Vector3.zero;
        }

        // orient forward towards target
        var newForward = desiredPosition - transform.position;
        var newRight = Vector3.Cross(transform.up, newForward);
        var newUp = Vector3.Cross(newForward, newRight);
        var rotation = Quaternion.LookRotation(newForward, transform.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 180 * Time.deltaTime);

        if (PathIsFree())
        {
            if(type == Type.medusa)
                transform.rotation = Quaternion.AngleAxis(90 * Time.deltaTime * Mathf.Cos(Time.realtimeSinceStartup), transform.forward) * transform.rotation;
            if (canSwim)
            {
                if(type == Type.shark)
                    transform.position += transform.forward * 10f * Time.deltaTime;
                if(type == Type.medusa)
                    transform.position += transform.forward * 3f * Time.deltaTime;
            }
        }
    }

    public float playerBoundRadius = 2f;
    void AttackPlayer()
    {
        GoTowards(Access.Player().transform.position);
    }

    public Vector3 targetPosition;
    bool TargetReached()
    {
        return (targetPosition - transform.position).magnitude < playerBoundRadius;
    }

    bool PathIsFree()
    {
        return !Physics.Raycast(transform.position, transform.forward, 3f, gameObject.layer);
    }

    bool WallReached()
    {
        return bc ? !bc.bounds.Contains(rb.position) : false;
    }
    void RandomMove()
    {
        if (TargetReached() && canSeePlayer) {
            player.DoDamage( Type.shark == type ? 10 : 1);
        }
        if (currentTimerRndDirChg >= timerRndDirChg || (TargetReached()))
        {
            // chose random pos in boundaries
            var newDir = Random.insideUnitSphere * GetComponent<SphereCollider>().radius;
            var newDesiredTarget = transform.position + newDir;
            GoTowards(newDesiredTarget);
            currentTimerRndDirChg = 0;
        }
        else
        {
            GoTowards(targetPosition);
        }
    }
}
