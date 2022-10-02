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

    ///
    private float elapsedTimeBeforeDeletion = 0f;
    public Transform    impalingObject;
    public Vector3      impalingLocalisation;
    public Vector3      impalingNormal;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        isDead = false;
        elapsedTimeBeforeDeletion = 0f;
        
        impalingObject = null;
        impalingLocalisation = Vector3.zero;
        impalingNormal = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
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

}
