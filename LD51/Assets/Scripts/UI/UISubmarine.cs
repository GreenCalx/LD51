using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISubmarine : MonoBehaviour
{
    [Header("Mandatory")]
    public Image healthBarRef;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateHullHealth(float HPRatio)
    {
        healthBarRef.fillAmount = HPRatio;
    }
}
