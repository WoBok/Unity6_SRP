using UnityEngine;
using UnityEngine.Rendering;

namespace LiteRP.FrameData
{
    public class CameraData : ContextItem//使用ContextContainer管理，为了防止创建新对象重复申请内存，每次ContextContainer Dispose时ResetContextItem对象，以重复利用
    {
        public Camera camera;
        public CullingResults cullingResults;
        public override void Reset()
        {
            camera = null;
            cullingResults = default;
        }
    }
}