using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProcAnim : MonoBehaviour
{
    public List<Transform> bones;
    public List<Vector3> bones_last_position;
    public List<List<Spring>> bones_springs;
    public float limitAngle;
    public float rootAngle;
    private float currentTime;
    public Spring S;
    // Start is called before the first frame update
    void Start()
    {
        bones_last_position = new List<Vector3>();
        bones_springs = new List<List<Spring>>();
        foreach(var b in bones) {
            bones_last_position.Add(b.position);
            List<Spring> newListXYZ = new List<Spring>();
            newListXYZ.Add(S);
            newListXYZ.Add(S);
            newListXYZ.Add(S);
            bones_springs.Add(newListXYZ);
        }
    }

    public static Quaternion ClampRotation(Quaternion q, Vector3 bounds, ref List<Spring> S)
{

    #if false
    q.x /= q.w;
    q.y /= q.w;
    q.z /= q.w;
    q.w = 1.0f;
 
    float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
    angleX = Mathf.Clamp(angleX, -bounds.x, bounds.x);
    q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);
 
    float angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.y);
    angleY = Mathf.Clamp(angleY, -bounds.y, bounds.y);
    q.y = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleY);
 
    float angleZ = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.z);
    angleZ = Mathf.Clamp(angleZ, -bounds.z, bounds.z);
    q.z = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleZ);
 
    return q.normalized;
    #endif

    var euler = q.eulerAngles;
    if (euler.x > 180) euler.x = euler.x - 360f;
    if (euler.x < -180) euler.x = euler.x + 360f;

    if (euler.y > 180) euler.y = euler.y - 360f;
    if (euler.y < -180) euler.y = euler.y + 360f;

    if (euler.z > 180) euler.z = euler.z - 360f;
    if (euler.z < -180) euler.z = euler.z + 360f;

    euler.x = Mathf.Clamp(euler.x, -bounds.x, bounds.x);
    euler.y = Mathf.Clamp(euler.y, -bounds.y, bounds.y);
    euler.z = Mathf.Clamp(euler.z, -bounds.z, bounds.z);

    euler.x = S[0].GetValue( euler.x / bounds.x, Time.deltaTime ) * bounds.x;
    euler.y = S[1].GetValue( euler.y / bounds.y, Time.deltaTime ) * bounds.y;
    euler.z = S[2].GetValue( euler.z / bounds.z, Time.deltaTime ) * bounds.z;

    return Quaternion.Euler(euler);
}

    // Update is called once per frame
    void FixedUpdate()
    {
        currentTime += Time.deltaTime;

        for(int i = 0; i <= bones.Count-2; ++i) {
            // foreach bones
            var root = bones[i].transform;

            // constraints resolution
            var constraints = bones_last_position[i+1];
            var constraintsDir = constraints - root.position;
            var newRight = Vector3.Cross(constraintsDir, root.forward);
            var newForward = Vector3.Cross(newRight, constraintsDir);
            var angle = Quaternion.LookRotation(newForward, constraintsDir);
            root.rotation = angle;
            // velocity resolution
            var velocity = bones[i+1].position - bones_last_position[i+1];
            var nextTarget = bones[i+1].position + velocity*Time.deltaTime;
            var dirLastRoot = nextTarget - root.position;

            newRight = Vector3.Cross(dirLastRoot, root.forward);
            newForward = Vector3.Cross(newRight, dirLastRoot);
            angle = Quaternion.LookRotation(newForward, dirLastRoot);
            root.rotation = angle;
            // clamp rotations
            var spring = bones_springs[i];
            var newAngles = ClampRotation(root.localRotation, new Vector3(limitAngle, limitAngle, limitAngle), ref spring);
            bones_springs[i] = spring;

            root.localRotation = newAngles;
            bones_last_position[i] = root.position;       
        }
        bones_last_position[bones.Count-1] = bones[bones.Count-1].position;
    }
}
