using UnityEngine;
using UnityEngine.Rendering;

public class RenderingEventTest : MonoBehaviour
{
    void Start()
    {
        RenderPipelineManager.beginCameraRendering += BeginCameraRendering;
    }

    void BeginCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        Debug.Log("BeginCameraRendering");
    }
}