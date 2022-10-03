using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpoon : MonoBehaviour
{
    public bool isFired = false;
    private Rigidbody rb;
    public ParticleSystem bleedPS;

    // Start is called before the first frame update
    void Awake()
    {
        init();
    }

    private void init()
    {
        isFired = false; 
        rb = GetComponent<Rigidbody>();
        bleedPS.Pause();
        bleedPS.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFired)
        {
            // Propelllllllllllll

        }
    }

    public void launch(Vector3 iDirection, float shootingForce)
    {
        if (rb==null)
            init();
        rb.AddForce( iDirection * shootingForce, ForceMode.VelocityChange );
    }

    void OnCollisionEnter(Collision other)
    {
        Enemy e = other.transform.gameObject.GetComponent<Enemy>();
        if (e!=null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero; 
            rb.isKinematic = true;
            rb.mass = 0;

            Vector3 collisionNormal = (other.transform.position - transform.position).normalized;
            ContactPoint cp = other.contacts[0];
            
            if (!e.isDead)
            { 
                e.impale( transform, cp.point, collisionNormal); 
                transform.SetParent(e.transform);
                bleedPS.gameObject.SetActive(true);
                bleedPS.Play();
                rb.isKinematic = true;
            }
        }
        else {
            Destroy(gameObject); // hit something like a wall
        }
    }
}
