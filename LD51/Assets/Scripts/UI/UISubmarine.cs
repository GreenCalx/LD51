using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISubmarine : MonoBehaviour
{
    [Header("Mandatory")]
    public Image healthBarRef;
    public Image echoLocalisationRef;

    ///
    private float echoElapsedTime;


    // Start is called before the first frame update
    void Start()
    {
        echoElapsedTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        updateEchoLocalisationHint();
    }

    public void updateHullHealth(float HPRatio)
    {
        healthBarRef.fillAmount = HPRatio;
    }

    public void updateEchoLocalisationHint()
    {
        echoElapsedTime += Time.deltaTime;
        
        float fillRatio = echoElapsedTime / Constants.SACRED_NUMBER;
        fillRatio = (fillRatio>=1f) ? 1f : fillRatio;
        echoLocalisationRef.transform.localScale = new Vector3(fillRatio,fillRatio,fillRatio);
        
        if ( echoElapsedTime >= Constants.SACRED_NUMBER)
        { pulseActive(); echoElapsedTime = 0f; }


    }

    private void pulseActive()
    {
        // for effects if we need
    }
}
