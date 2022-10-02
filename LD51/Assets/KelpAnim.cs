using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KelpAnim : MonoBehaviour
{
    float currentTime = 0;
    Quaternion baseRot;
    public float angle;
    public float amplitude;
    // Start is called before the first frame update
    void Start()
    {
        baseRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        // NOTE toffa : sens d'application des rotations very important!
        transform.rotation = Quaternion.AngleAxis( angle * Mathf.Cos(currentTime*amplitude) * Time.deltaTime, transform.forward ) * transform.rotation;   
    }
}
