using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITutorial : MonoBehaviour
{
    public float minimumDuration = 2f;
    private float elapsedTime;
    public PlayerController controller;

    void Awake()
    {
        elapsedTime = 0f;
        if (controller==null)
            controller = Access.Player();
        controller.processInputs = false;
        Time.timeScale = 0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.unscaledDeltaTime;
        if ( elapsedTime >= minimumDuration )
        {
            if ( Input.anyKey )
            {
                Destroy(gameObject); // tutorial is over
                controller.processInputs = true;
                Time.timeScale = 1f;
                Access.SoundManager().Play(Constants.BGM_BLOOP);
            }
        }
    }
}
