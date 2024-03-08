using UnityEngine;
using UnityEngine.Rendering;

public class SetupLiteRP : MonoBehaviour
{
    public RenderPipelineAsset assets;
    void OnEnable()
    {
        SetAssets();
    }
    void OnValidate()
    {
        SetAssets();
    }
    void SetAssets()
    {
        if (assets != null)
        {
            GraphicsSettings.renderPipelineAsset = assets;
        }
    }
}