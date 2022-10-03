using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{

    public string audioToPlay;
    public bool oneShot = true;

    void OnTriggerEnter()
    {
        if (oneShot)
            Access.SoundManager().PlayOneShot(audioToPlay);
        else
            Access.SoundManager().Play(audioToPlay);
    }
}
