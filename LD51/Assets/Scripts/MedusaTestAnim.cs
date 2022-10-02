using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Radar {
    public static bool IsUnderRadar() {
        return true;
    }
}

public class MedusaTestAnim : MonoBehaviour
{
    float currentTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        // NOTE toffa : sens d'application des rotations very important!
        //transform.rotation = Quaternion.AngleAxis( 45* Time.deltaTime, transform.forward ) * transform.rotation;    

        //transform.position += transform.right * Time.deltaTime * Mathf.Sin(currentTime*2) * 5;
        //transform.position += transform.forward * Time.deltaTime * Mathf.Sin(currentTime);    
    }
}
