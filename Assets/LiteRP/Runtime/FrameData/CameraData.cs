using UnityEngine;
using UnityEngine.Rendering;

namespace LiteRP.FrameData
{
    public class CameraData : ContextItem//ʹ��ContextContainer����Ϊ�˷�ֹ�����¶����ظ������ڴ棬ÿ��ContextContainer DisposeʱResetContextItem�������ظ�����
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