using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineWeapon : MonoBehaviour
{
    
    [Header("Mandatory")]
    public Transform    harpoonLauncher;
    public Transform    harpoonAnchor;
    public GameObject   harpoonAmmoRef;

    [Header("Tweaks")]
    [Range(0.01f,50f)]
    public float shootingPower;
    
    [HideInInspector]
    public GameObject currHarpoon;
    private Harpoon currHarpoonAsHarpoon;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void spawnAmmo()
    {
        currHarpoon = Instantiate(harpoonAmmoRef);
        currHarpoon.transform.position = harpoonAnchor.position;
        currHarpoonAsHarpoon = currHarpoon.GetComponent<Harpoon>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!!currHarpoon)
        {
            currHarpoon.transform.position = harpoonAnchor.position;
        }
    }

    public void fire()
    {
        currHarpoonAsHarpoon.launch( -transform.forward.normalized, shootingPower );
        currHarpoonAsHarpoon.isFired = true;

        currHarpoon = null;
        currHarpoonAsHarpoon = null;
    }

        
}
