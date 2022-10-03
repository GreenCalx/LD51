using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCinematic : MonoBehaviour
{
    public GameObject player;
    public GameObject portal;
    public float speed = 1.0F;

    private float startTime;
    private float journeyLength;
    private PlayerController PC;
    private Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        PC = player.GetComponent<PlayerController>();
        PC.processInputs = false;

        Rigidbody prb = player.GetComponent<Rigidbody>();
        prb.isKinematic = true;
        startPos = player.transform.position;

        // Keep a note of the time the movement started.
        startTime = Time.time;

        // Calculate the journey length.
        journeyLength = Vector3.Distance(player.transform.position, portal.transform.position);
    }

    // Move to the target end position.
    void Update()
    {
        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - startTime) * speed;

        // Fraction of journey completed equals current distance divided by total distance.
        float fractionOfJourney = distCovered / journeyLength;

        // Set our position as a fraction of the distance between the markers.
        player.transform.position = Vector3.Lerp(startPos, portal.transform.position, fractionOfJourney);

        if (Vector3.Distance(player.transform.position, portal.transform.position) < 1f)
        {
            SceneManager.LoadScene(Constants.SN_TITLE, LoadSceneMode.Single);
        }
    }
}
