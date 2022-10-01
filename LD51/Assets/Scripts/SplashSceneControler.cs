using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SplashSceneControler : MonoBehaviour
{
    public float loadingTime;
    private float currentTime;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= loadingTime || Input.GetKeyDown("Fire")) LoadScene();
    }

    void LoadScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
