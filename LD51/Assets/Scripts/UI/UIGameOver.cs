using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameOver : MonoBehaviour
{
    public float time_before_restart = 1f;
    private float elapsed_time = 0f;
    // Start is called before the first frame update
    void Start()
    {
        elapsed_time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        elapsed_time += Time.deltaTime;
        if ( elapsed_time < time_before_restart)
            return;
        if (Input.anyKey)
        {
            SceneManager.LoadScene( Constants.SN_TITLE, LoadSceneMode.Single);
        }
    }
}
