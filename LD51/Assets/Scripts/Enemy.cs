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


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        isDead = false;
        elapsedTimeBeforeDeletion = 0f;
        
        impalingObject = null;
        impalingLocalisation = Vector3.zero;
        impalingNormal = Vector3.zero;

        currentSwitchLatch = swimLatch;
    }

    public bool canSwim = false;

    // Update is called once per frame
    void Update()
    {
        currentSwitchLatch -= Time.deltaTime;
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

public float playerBoundRadius = 2f;
    void AttackPlayer() {
       var ppos = Access.Player().transform.position;
       var newForward = ppos - transform.position;
       var newRight = Vector3.Cross(transform.up, newForward);
       var newUp = Vector3.Cross(newForward, newRight);
       transform.rotation = Quaternion.LookRotation(newForward, transform.up);
       
       if (newForward.magnitude < playerBoundRadius) {
            transform.position -= (playerBoundRadius-newForward.magnitude)*newForward;
            return;
       }

       transform.rotation = Quaternion.AngleAxis( 90 * Time.deltaTime * Mathf.Cos(Time.realtimeSinceStartup), transform.forward ) * transform.rotation;
       if (canSwim) {
           transform.position += transform.forward * 3f * Time.deltaTime;
       }
    }
}
