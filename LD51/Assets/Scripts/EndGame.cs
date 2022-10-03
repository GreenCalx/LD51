using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    void OnTriggerEnter(Collider iCol)
    {
        if (!!iCol.GetComponent<PlayerController>())
        {
            SceneManager.LoadScene(Constants.SN_TITLE, LoadSceneMode.Single);
        }
    }
}
