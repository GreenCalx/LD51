using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class radar : MonoBehaviour
{
    public Material shader;
    public Material shaderEnnemy;
    public Transform playerPosition;
    float CurrentTime = 0;

    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
         cam = GetComponent<Camera>();
        cam.depthTextureMode = cam.depthTextureMode | DepthTextureMode.Depth;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.R) || CurrentTime >= 10f) {
            CurrentTime = 0;
            
        }
        shader.SetVector("_PlayerPosition", playerPosition.position);
        shaderEnnemy.SetVector("_PlayerPosition", playerPosition.position);
        shader.SetFloat("_CurrentTime", CurrentTime);
        shaderEnnemy.SetFloat("_CurrentTime", CurrentTime);
    }

#if false
      private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Matrix4x4 projectionMatrix = cam.projectionMatrix;
        projectionMatrix = GL.GetGPUProjectionMatrix(projectionMatrix, false);
        projectionMatrix.SetColumn(1, projectionMatrix.GetColumn(1)*-1);

        Matrix4x4 viewMatrix = cam.worldToCameraMatrix;

        shader.SetMatrix("_UNITY_MATRIX_I_V", viewMatrix.inverse);
        shader.SetMatrix("_UNITY_MATRIX_I_P", projectionMatrix.inverse);
        shader.SetMatrix("_UNITY_MATRIX_I_VP", (projectionMatrix * viewMatrix).inverse);
        Graphics.Blit(source, destination, shader);
    }
    #endif
}
