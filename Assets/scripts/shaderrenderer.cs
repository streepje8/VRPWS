using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[ImageEffectAllowedInSceneView]
public class shaderrenderer : MonoBehaviour
{
    public Material shader;

    private void Start()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, shader);
    }

}
