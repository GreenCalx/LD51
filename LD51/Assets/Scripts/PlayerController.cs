using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

[Header("Inputs")]
public bool processInputs = true;
[Header("Inputs/UI")]
public GameObject pauseMenu;
   
    [Header("Tweaks")]
    [Range(0f,100f)]
    public float verticalPropellerStrength = 1f;
    [Range(0f,100f)]
    public float horizontalPropellerStrength = 1f;
    [Range(1f,180f)]
    public float rotSpeedDegPerSec = 1f;
    [Range(0f,10f)]
    public float pitchDragStrength = 1f;
    [Range(0f,45f)]
    public float MAX_PITCH_DEG = 30f;
    [Range(0f,10f)]
    public float MAX_SPEED = 30f;
    [Range(0.1f,5f)]
    public float timeBeforePitchDragStarts = 1f; // seconds

    [Header("Mandatory Refs")]
    public Transform selfFrontRef;
    public Transform selfRearRef;

    ///
    private Rigidbody rb;
    private float currentPitch = 0f;
    private float timeVertPropActivated = 0f;
    private float timeHorPropActivated = 0f;
    private float initialPitch = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        timeVertPropActivated = 0f;
        timeHorPropActivated = 0f;

        initialPitch = rb.rotation.eulerAngles.x;
        updateCurrentPitch();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        turnVelocity = new Vector3(0f, 0f, rotSpeedDegPerSec);
        if(processInputs) updateInputs();
        submarineEffects();
    }

    void Update() {
        if (!processInputs) return;

        if(Input.GetButtonDown("Cancel")) {
            // spawn pause menu
            Time.timeScale = 0f;
            processInputs = false;
            pauseMenu.SetActive(true);
        }
    }

    private void updateInputs()
    {
        // vertical propeller (upward)
        if ( Input.GetButton(Constants.INPUT_VPROP))
        {
            rb.AddForce(new Vector3(0,verticalPropellerStrength,0), ForceMode.Force);
            timeVertPropActivated += Time.fixedDeltaTime;
        } else {
            timeVertPropActivated = 0f;
        }

        // horizontal propeller (forward)
        if (Input.GetAxisRaw(Constants.INPUT_HPROP) > 0)
        {
            Vector3 direction = selfFrontRef.position - selfRearRef.position;
            direction *=  horizontalPropellerStrength;
            
            // check if current speed > max speed
            if ( rb.velocity.magnitude >= MAX_SPEED )
            {
                rb.velocity = rb.velocity.normalized * MAX_SPEED;
            } else {
                // if not we add force
                rb.AddForce( direction, ForceMode.Force);
            }
            timeHorPropActivated += Time.fixedDeltaTime;
        } else {
            timeHorPropActivated = 0f;
        }

        // -horizontal propeller (brake)
        if (Input.GetAxisRaw(Constants.INPUT_HPROP) < 0)
        {
            Vector3 direction = selfFrontRef.position - selfRearRef.position;
            direction *=  horizontalPropellerStrength;
            rb.AddForce( -direction, ForceMode.Force);
        }

        // Turn (L/R)
        if ( Input.GetButton(Constants.INPUT_TURN) )
        {
            float turnDirection = Input.GetAxisRaw(Constants.INPUT_TURN);
            turnDirection = (turnDirection >= 0) ? 1 : -1;
            Quaternion deltaRot = Quaternion.Euler( turnDirection * new Vector3(0f, 0f, rotSpeedDegPerSec) * Time.fixedDeltaTime);
            rb.MoveRotation( rb.rotation * deltaRot );
        }
    }

    private void submarineEffects()
    {
        // pitch drag IF VERT + horizontal speed
        if ( (timeVertPropActivated >= timeBeforePitchDragStarts ) && 
             (currentPitch < MAX_PITCH_DEG) &&
             (timeHorPropActivated >= timeBeforePitchDragStarts)
           )
        {
            // crank up pitch
            Vector3 pitchRot = new Vector3(pitchDragStrength, 0f, 0f);
            Quaternion deltaPitchRot = Quaternion.Euler( pitchRot * Time.fixedDeltaTime);
            rb.MoveRotation( rb.rotation * deltaPitchRot );

            updateCurrentPitch();

        } else if ((currentPitch > 0f)&&(timeVertPropActivated<timeBeforePitchDragStarts))
        {
            // restabilize pitch to 0
            Vector3 pitchRot = new Vector3(-pitchDragStrength, 0f, 0f);
            
            float yDeltaFrontRear = selfFrontRef.position.y - selfRearRef.position.y;
            if (yDeltaFrontRear > 0)
            { Debug.Log("good ptch");}
            else if (yDeltaFrontRear <= 0)
            {
                rb.rotation = Quaternion.Euler( new Vector3( initialPitch, rb.rotation.eulerAngles.y, rb.rotation.eulerAngles.z));
                return;
            }
            // handle the case where next rot attains 0 pitch to avoid
            // going through it and find ourselves diving
            // float remainingRot = currentPitch - pitchDragStrength;
            // if (remainingRot < 0)
            // {
            //     pitchRot = new Vector3( -currentPitch,0,0);
            // }
            Quaternion deltaPitchRot = Quaternion.Euler( pitchRot * Time.fixedDeltaTime);
            rb.MoveRotation( rb.rotation * deltaPitchRot );

            updateCurrentPitch();
        }
        


    }

    private void updateCurrentPitch()
    {
        currentPitch = rb.rotation.eulerAngles.x - initialPitch;
    }
}

