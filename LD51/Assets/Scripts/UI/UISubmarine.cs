using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISubmarine : MonoBehaviour
{
    [Header("Mandatory")]
    public Image healthBarRef;
    public Image echoLocalisationRef;
    public Transform ammoImagesHolderRef;

    [Header("UI")]
    public Color  colorScale;
    public Sprite availableAmmo;
    public Sprite usedAmmo;
    public TextMeshProUGUI harpoonReady;
    ///
    private float echoElapsedTime;
    private Image[] ammoImageRefs;

    // Start is called before the first frame update
    void Start()
    {
        echoElapsedTime = 0f;
        ammoImageRefs = ammoImagesHolderRef.GetComponentsInChildren<Image>();
        initChildImagesColors(transform);
    }

    public void setHarpoonRdy(bool iState)
    {
        harpoonReady.gameObject.SetActive(iState);
    }

    public void initChildImagesColors(Transform root)
    {
        foreach (Transform child in root) {

            if (child.name=="Tutorial")
                continue; // no colorize tuto

            Image im = child.GetComponent<Image>();
            Debug.Log(child.name);
            if (!!im)
                im.color = colorScale;
            
            TextMeshProUGUI txt = child.GetComponent<TextMeshProUGUI>();
            if (!!txt)
                txt.color = colorScale;

            if (child.childCount>0)
                initChildImagesColors(child);
        }
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

    public void refreshAmmos(int remainingAmmos)
    {
        for (int i = ammoImageRefs.Length-1; i >= 0; i--)
        {
            if ( i < remainingAmmos)
                ammoImageRefs[i].sprite = availableAmmo;
            else
                ammoImageRefs[i].sprite = usedAmmo;
        }
    }
}
