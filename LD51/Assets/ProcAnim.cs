using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Spring {
    float min;
    float max;
    float dampRatio;
    float stiffness;
    float rest;
    float vel;

    float GetValue(float position, float deltaTime) {
        float x = position - rest;
		vel += (-dampRatio * vel) - (stiffness * x);
		position += vel * deltaTime;
        return position;
    }
}
public class ProcAnim : MonoBehaviour
{
    public List<Transform> bones;
    public float limitAngle;
    private float currentTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        for(int i = 0; i < bones.Count-2; ++i) {
            // foreach bones
            var root = bones[i].transform;
            var lastNextPos = bones[i+1].transform.position + 2f * bones[i+1].up;
            // apply changes
            if (i == 0) {
                root.rotation = Quaternion.AngleAxis(30 * Mathf.Sin(currentTime), root.forward);
            }
            // correct angle
            var nextDir = lastNextPos - root.position;
            var angle = Quaternion.LookRotation(root.forward, nextDir);

            bones[i+1].transform.rotation = angle;
        }
    }
}
