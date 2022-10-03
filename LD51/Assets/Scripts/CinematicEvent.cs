using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicEvent : MonoBehaviour
{
    public CinematicCallable cinematicCallable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider iCollider)
    {
        if (iCollider.transform.parent.GetComponent<PlayerController>())
        {
           cinematicCallable.OnCall() ;
        }
    }
}
