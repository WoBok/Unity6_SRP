using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace LiteRP
{
    public class LiteRenderPipeline : RenderPipeline
    {
        readonly static ShaderTagId s_ShaderTagId = new ShaderTagId("SRPDefaultUnlit");
        protected override void Render(ScriptableRenderContext context, Camera[] cameras)
        {

        }
        protected override void Render(ScriptableRenderContext context, List<Camera> cameras)//在编辑器下和运行时的Scene和Game窗口每帧调用，在调用Camera.main.Render()时，也会每帧调用
        {
            BeginContextRendering(context, cameras);
            for (int i = 0; i < cameras.Count; i++)
            {
                RenderCamera(context, cameras[i]);
            }
            EndContextRendering(context, cameras);
        }

        void RenderCamera(ScriptableRenderContext context, Camera camera)
        {
            //此函数调用RenderPipelineManager.beginCameraRendering?.Invoke(context, camera)
            BeginCameraRendering(context, camera);

            if (!camera.TryGetCullingParameters(out var cullingParameters))
                return;
            CullingResults cullingResults = context.Cull(ref cullingParameters);

            CommandBuffer cmd = CommandBufferPool.Get(camera.name);
            context.SetupCameraProperties(camera);


            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
            CommandBufferPool.Release(cmd);
            context.Submit();

            EndCameraRendering(context, camera);
        }
        //Legacy mode
        void Render(ScriptableRenderContext context, Camera camera, CommandBuffer cmd)
        {
            if (!camera.TryGetCullingParameters(out var cullingParameters))
                return;
            CullingResults cullingResults = context.Cull(ref cullingParameters);

            var clearDepth = camera.clearFlags != CameraClearFlags.Nothing;
            var clearColor = camera.clearFlags == CameraClearFlags.Color;

            cmd.ClearRenderTarget(clearDepth, clearColor, CoreUtils.ConvertSRGBToActiveColorSpace(camera.backgroundColor));

            var clearSkybox = camera.clearFlags == CameraClearFlags.Skybox;
            if (clearSkybox)
            {
                var skyboxRendererList = context.CreateSkyboxRendererList(camera);
                cmd.DrawRendererList(skyboxRendererList);
            }

            var sortSettings = new SortingSettings(camera);
            var drawSettings = new DrawingSettings(s_ShaderTagId, sortSettings);
            sortSettings.criteria = SortingCriteria.CommonOpaque;
            var filterSettings = new FilteringSettings(RenderQueueRange.opaque);
            var rendererListParmas = new RendererListParams(cullingResults, drawSettings, filterSettings);

            var rendererList = context.CreateRendererList(ref rendererListParmas);
            cmd.DrawRendererList(rendererList);

            sortSettings.criteria = SortingCriteria.CommonTransparent;
            filterSettings.renderQueueRange = RenderQueueRange.transparent;
            rendererListParmas.filteringSettings = filterSettings;

            rendererList = context.CreateRendererList(ref rendererListParmas);
            cmd.DrawRendererList(rendererList);
        }
    }
}