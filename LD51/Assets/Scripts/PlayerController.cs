using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

   
    [Header("Tweaks")]
    [Range(0f,30f)]
    public float verticalPropellerStrength = 1f;
    [Range(0f,30f)]
    public float horizontalPropellerStrength = 1f;
    [Range(1f,180f)]
    public float rotSpeedDegPerSec = 1f;

    [Header("Mandatory Refs")]
    public Transform selfFrontRef;
    public Transform selfRearRef;

    ///
    private Rigidbody rb;
    private Vector3 turnVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        turnVelocity = new Vector3(0f, rotSpeedDegPerSec, 0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        turnVelocity = new Vector3(0f, 0f, rotSpeedDegPerSec);
        updateInputs();
    }

    private void updateInputs()
    {
        if ( Input.GetButton(Constants.INPUT_VPROP))
        {
            rb.AddForce(new Vector3(0,verticalPropellerStrength,0), ForceMode.Force);
        }
        if ( Input.GetButton(Constants.INPUT_HPROP))
        {
            Vector3 direction = selfFrontRef.position - selfRearRef.position;
            direction *=  horizontalPropellerStrength;
            rb.AddForce( direction, ForceMode.Force);
        }
        if ( Input.GetButton(Constants.INPUT_TURN) )
        {
            float turnDirection = Input.GetAxisRaw(Constants.INPUT_TURN);
            turnDirection = (turnDirection >= 0) ? 1 : -1;
            Quaternion deltaRot = Quaternion.Euler( turnDirection * turnVelocity * Time.fixedDeltaTime);
            rb.MoveRotation( rb.rotation * deltaRot );
        }
    }
}
