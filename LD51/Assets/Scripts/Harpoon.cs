using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpoon : MonoBehaviour
{
    public bool isFired = false;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        isFired = false; 
        rb = GetComponent<Rigidbody>();
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
        rb.AddForce( iDirection * shootingForce, ForceMode.VelocityChange );
    }

    void OnCollisionEnter(Collision other)
    {
        Enemy e = other.transform.gameObject.GetComponent<Enemy>();
        if (e!=null)
        {
            e.impale();
        }
    }
}
