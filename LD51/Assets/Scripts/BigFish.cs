using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BigFish : CinematicCallable
{
    public float speed;
    public Vector3 moveDirection;
    public float delayedResponse = 1f;

    private bool goSwallow = false;
    private Rigidbody rb;
    private float elapsedDelay = 0f;
    private bool delayActive = false;
    // Start is called before the first frame update
    void Start()
    {
        goSwallow = false;
        rb = GetComponent<Rigidbody>();
        elapsedDelay = 0f;
        delayActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (goSwallow)
        { moveInSetDirection(); }
        if (delayActive && goSwallow)
        {
            elapsedDelay += Time.deltaTime;
            if (elapsedDelay >= delayedResponse)
            { loadEntrails(); }
        }
    }

    void moveInSetDirection()
    {
        rb.AddForce(moveDirection * speed, ForceMode.VelocityChange);
    }

    void OnTriggerEnter(Collider iCollider)
    {
        Debug.Log("bug fish trigger enter");
        delayActive = true;
        elapsedDelay = 0f;
    }

    public void loadEntrails()
    {
        SceneManager.LoadScene( Constants.SN_FISH, LoadSceneMode.Single);
    }

    public override void OnCall()
    {
        goSwallow = true;
    }
}
