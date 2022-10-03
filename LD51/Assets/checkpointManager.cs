using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpointManager : MonoBehaviour
{
    public List<Transform> cps;
    public void Respawn(ref Transform tin) {
        // get current cp   
        foreach(var t in cps) {
            if (Vector3.Dot(t.forward, tin.position-t.position) > 0f) {
                tin.position = t.position;
                tin.forward = t.forward;
                Debug.Log("cp found");
                break;
            }
        }
    }
}
