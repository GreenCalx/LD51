using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenuControler : MonoBehaviour
{
    private float elapsedTime = 0f;
    public float lock_duration;
    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0f;   
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime+=Time.deltaTime;
        if (elapsedTime>lock_duration)
        {
            if (Input.anyKey)
            {
                LoadGame();
            }
        }
    }

    public void LoadGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void Exit() {
        Application.Quit();
    }
}
