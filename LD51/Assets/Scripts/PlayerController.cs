using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Inputs")]
    public bool processInputs = true;
    [Header("Inputs/UI")]
    public GameObject pauseMenu;

    [Header("Mandatory Refs")]
    public Transform selfFrontRef;
    public Transform selfRearRef;
    public UISubmarine submarineUI;
    public SubmarineWeapon submarineWeapon;
    public Damage3DAudio audioDamage;

    [Header("Control Tweaks")]
    [Range(0f, 100f)]
    public float verticalPropellerStrength = 1f;
    [Range(0f, 100f)]
    public float horizontalPropellerStrength = 1f;
    [Range(1f, 180f)]
    public float rotSpeedDegPerSec = 1f;
    [Range(0f, 10f)]
    public float pitchDragStrength = 1f;
    [Range(0f, 45f)]
    public float MAX_PITCH_DEG = 30f;
    [Range(0f, 10f)]
    public float MAX_SPEED = 1f;
    [Range(0.1f, 5f)]
    public float timeBeforePitchDragStarts = 1f; // seconds

    [Header("Submarine Tweaks")]
    public float MAX_HP = 4f;
    public AnimationCurve hullDamageOverSpeed;
    public int MAX_AMMO = 5;
    public int currAmmo;


    ///
    private Rigidbody rb;
    private float currentPitch = 0f;
    private float timeVertPropActivated = 0f;
    private float timeHorPropActivated = 0f;
    private float initialPitch = 0f;

    private float currSpeed = 0f;
    private float currHP;
    private Vector3 playerVelocity;

    private float reloadCooldown;

    public Spring UpVectorSpring;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currSpeed = 0f;
        currHP = MAX_HP;
        currAmmo = MAX_AMMO;
        playerVelocity = rb.velocity;

        reloadCooldown = 0f;

        timeVertPropActivated = 0f;
        timeHorPropActivated = 0f;

        initialPitch = rb.rotation.eulerAngles.x;
        updateCurrentPitch();

    }

    public float GetDegats()
    {
        return 1 - Mathf.Clamp01(currHP / MAX_HP);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerVelocity = rb.velocity;

        // if not we add force
        rb.AddForce(nextFrameForce);
        rb.AddTorque(nextFrameTorque);

        // check if current speed > max speed
        rb.velocity = rb.velocity.normalized * Mathf.Clamp(rb.velocity.magnitude, 0, MAX_SPEED);
        submarineEffects();
        currSpeed = new Vector3(rb.velocity.x * rb.transform.forward.x, rb.velocity.y * rb.transform.forward.y, rb.velocity.z * rb.transform.forward.z).magnitude;

        nextFrameForce = Vector3.zero;
        nextFrameTorque = Vector3.zero;

        // correct up vector
        // always force to have up vector along world up
        var worldUp = Vector3.up;
        var currentUp = transform.up;
        var rot = Quaternion.FromToRotation(currentUp, worldUp);
        float angle = 0.0f;
        Vector3 axis = Vector3.zero;
        rot.ToAngleAxis(out angle, out axis);
        if (angle > 180) angle -= 360;
        if (angle < -180) angle += 360;
        var error = angle / 180;
        angle = UpVectorSpring.GetValue(error, Time.fixedDeltaTime) * 180;
        var newRot = Quaternion.AngleAxis(angle, axis);
        rb.MoveRotation(rb.rotation * newRot);
    }

    void Update()
    {
        if (!processInputs) return;
        updateInputs();

        if (Input.GetButtonDown("Cancel"))
        {
            // spawn pause menu
            Time.timeScale = 0f;
            processInputs = false;
            pauseMenu.SetActive(true);
        }

        if (currHP <= 0)
        { kill(); }

        if (reloadCooldown > 0f)
        {
            reloadCooldown -= Time.deltaTime;
            submarineUI.setHarpoonRdy(false);
        }
        else
        {
            submarineUI.setHarpoonRdy(true);
        }
    }

    private Vector3 nextFrameForce;
    private Vector3 nextFrameTorque;
    private Quaternion nextRot;

    private void updateInputs()
    {
        // vertical propeller (upward)
        if (Input.GetButton(Constants.INPUT_VPROP))
        {
            nextFrameForce += new Vector3(0, verticalPropellerStrength, 0);
            timeVertPropActivated += Time.deltaTime;
        }
        else
        {
            timeVertPropActivated = 0f;
        }

        // horizontal propeller (forward)
        if (Input.GetAxisRaw(Constants.INPUT_HPROP) > 0)
        {
            Vector3 direction = selfFrontRef.position - selfRearRef.position;
            direction *= horizontalPropellerStrength;

            nextFrameForce += direction;
            timeHorPropActivated += Time.fixedDeltaTime;
        }
        else
        {
            timeHorPropActivated = 0f;
        }

        // -horizontal propeller (brake)
        if (Input.GetAxisRaw(Constants.INPUT_HPROP) < 0)
        {
            Vector3 direction = selfFrontRef.position - selfRearRef.position;
            direction *= horizontalPropellerStrength;

            nextFrameForce += direction;
        }

        // Turn (L/R)
        if (Input.GetButton(Constants.INPUT_TURN))
        {
            float turnDirection = Input.GetAxisRaw(Constants.INPUT_TURN);
            turnDirection = (turnDirection >= 0) ? 1 : -1;

            nextFrameTorque += turnDirection * rotSpeedDegPerSec * transform.up;
        }

        // Fire Harpoon
        if (Input.GetButton(Constants.INPUT_FIRE))
        {
            tryShoot();
        }

        if (uiGameOver.active)
        {
            if (Input.GetButtonDown(Constants.INPUT_VPROP))
            {
                uiGameOver.SetActive(false);
                var t = transform;
                Access.CheckpointMgr().Respawn(ref t);
            }
        }
    }

    private void submarineEffects()
    {
        // pitch drag IF VERT + horizontal speed
        if ((timeVertPropActivated >= timeBeforePitchDragStarts) &&
             (currentPitch < MAX_PITCH_DEG) &&
             (timeHorPropActivated >= timeBeforePitchDragStarts)
           )
        {
            // crank up pitch
            Vector3 pitchRot = new Vector3(pitchDragStrength, 0f, 0f);
            Quaternion deltaPitchRot = Quaternion.Euler(pitchRot * Time.fixedDeltaTime);
            rb.MoveRotation(rb.rotation * deltaPitchRot);

            updateCurrentPitch();

        }
        else if ((currentPitch > 0f) && (timeVertPropActivated < timeBeforePitchDragStarts))
        {
            // restabilize pitch to 0
            Vector3 pitchRot = new Vector3(-pitchDragStrength, 0f, 0f);

            float yDeltaFrontRear = selfFrontRef.position.y - selfRearRef.position.y;
            if (yDeltaFrontRear > 0)
            { }
            else if (yDeltaFrontRear <= 0)
            {
                rb.rotation = Quaternion.Euler(new Vector3(initialPitch, rb.rotation.eulerAngles.y, rb.rotation.eulerAngles.z));
                return;
            }
            // handle the case where next rot attains 0 pitch to avoid
            // going through it and find ourselves diving
            // float remainingRot = currentPitch - pitchDragStrength;
            // if (remainingRot < 0)
            // {
            //     pitchRot = new Vector3( -currentPitch,0,0);
            // }
            Quaternion deltaPitchRot = Quaternion.Euler(pitchRot * Time.fixedDeltaTime);
            rb.MoveRotation(rb.rotation * deltaPitchRot);

            updateCurrentPitch();
        }



    }

    private void updateCurrentPitch()
    {
        currentPitch = rb.rotation.eulerAngles.x - initialPitch;
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("player collision enter");
        // get the direction from player to collider
        Vector3 collisionNormal = (other.transform.position - transform.position).normalized;
        // player speed towerds collision
        float playerCollisionSpeed = Vector3.Dot(collisionNormal, playerVelocity);
        if (playerCollisionSpeed < 0)
        { playerCollisionSpeed = 0; }

        // other collider's speed if its a moving object
        float otherCollisionSpeed = 0f;
        if (other.relativeVelocity.magnitude > playerCollisionSpeed)
        {
            otherCollisionSpeed = Vector3.Dot(-collisionNormal, other.relativeVelocity + playerVelocity);
        }

        if (otherCollisionSpeed <= 0.1f)
        { takeDamageFromStaticCollision(playerCollisionSpeed); }

        ContactPoint cp = other.contacts[0];

        playSpatializedCollisionAudio(cp.point);
        
    }

    private void playSpatializedCollisionAudio(Vector3 CP)
    {
        Vector3 crp = transform.InverseTransformPoint(CP);
        Debug.Log(crp);
        // Get the biggest relative coord => consider its coming from this direction
        float X_abs = Mathf.Abs(crp.x);
        float Y_abs = Mathf.Abs(crp.y);
        float Z_abs = Mathf.Abs(crp.z);

        bool is_X = (X_abs >= Y_abs) && (X_abs >= Z_abs);
        bool is_Y = (Y_abs >= X_abs) && (Y_abs >= Z_abs);
        bool is_Z = (Z_abs >= X_abs) && (Z_abs >= Y_abs);

        // Play right AS according to relative coord sign
        if (is_X)
        {
            if (crp.x > 0) // R
                Access.SoundManager().PlayOneShot(Constants.SFX_DAMAGERIGHT);
            else
                Access.SoundManager().PlayOneShot(Constants.SFX_DAMAGELEFT);
            return;
        }
        else if (is_Y)
        {
            if (crp.y > 0) // R
                Access.SoundManager().PlayOneShot(Constants.SFX_DAMAGEUP);
            else
                Access.SoundManager().PlayOneShot(Constants.SFX_DAMAGEDOWN);
            return;
        }
        else
        {
            if (crp.z > 0) // R
                Access.SoundManager().PlayOneShot(Constants.SFX_DAMAGEUPFRONT);
            else
                Access.SoundManager().PlayOneShot(Constants.SFX_DAMAGEUPBACK);
            return;
        }

    }

    private void takeDamageFromStaticCollision(float playerCollisionSpeed)
    {
        float speedRatio = playerCollisionSpeed / MAX_SPEED;
        float damage = hullDamageOverSpeed.Evaluate(speedRatio);
        currHP -= damage;

        if (!!submarineUI)
            submarineUI.updateHullHealth(currHP / MAX_HP);

    }

    public GameObject uiGameOver;
    private void kill()
    {
        //GAME OVER
        //uiGameOver.SetActive(true);
        //SceneManager.LoadScene( Constants.SN_GAMEOVER, LoadSceneMode.Single);
    }

    public void loadWeapon()
    {
        if (currAmmo > 0)
        {
            submarineWeapon.spawnAmmo();
        }
    }

    public void tryShoot()
    {
        if (reloadCooldown > 0f)
            return; // on CD

        if (currAmmo > 0)
        {
            submarineWeapon.spawnAmmo();
            reloadCooldown = Constants.SACRED_NUMBER;
            submarineWeapon.fire();
            currAmmo--;
            submarineUI.refreshAmmos(currAmmo);
            submarineUI.setHarpoonRdy(false);
        }
    }
}

