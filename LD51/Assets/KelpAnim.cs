using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KelpAnim : MonoBehaviour
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
        transform.rotation = Quaternion.AngleAxis( 45 * Mathf.Sin(currentTime) * Time.deltaTime, transform.forward ) * transform.rotation;   
    }
}
