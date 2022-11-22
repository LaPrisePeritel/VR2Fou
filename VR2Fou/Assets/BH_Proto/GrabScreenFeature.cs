using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GrabScreenFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class Settings
    {
        public string TextureName = "_GrabPassTransparent";
        public LayerMask LayerMask;
    }

    private class GrabPass : ScriptableRenderPass
    {
        private RenderTargetHandle tempColorTarget;
        private Settings settings;

        private RenderTargetIdentifier cameraTarget;

        public GrabPass(Settings s)
        {
            settings = s;
            renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
            tempColorTarget.Init(settings.TextureName);
        }

        public void Setup(RenderTargetIdentifier cameraTarget)
        {
            this.cameraTarget = cameraTarget;
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            cmd.GetTemporaryRT(tempColorTarget.id, cameraTextureDescriptor);
            cmd.SetGlobalTexture(settings.TextureName, tempColorTarget.Identifier());
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get();
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            Blit(cmd, cameraTarget, tempColorTarget.Identifier());

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(tempColorTarget.id);
        }
    }

    private class RenderPass : ScriptableRenderPass
    {
        private Settings settings;
        private List<ShaderTagId> m_ShaderTagIdList = new List<ShaderTagId>();

        private FilteringSettings m_FilteringSettings;
        private RenderStateBlock m_RenderStateBlock;

        public RenderPass(Settings settings)
        {
            this.settings = settings;
            renderPassEvent = RenderPassEvent.AfterRenderingTransparents + 1;

            m_ShaderTagIdList.Add(new ShaderTagId("SRPDefaultUnlit"));
            m_ShaderTagIdList.Add(new ShaderTagId("UniversalForward"));
            m_ShaderTagIdList.Add(new ShaderTagId("LightweightForward"));

            m_FilteringSettings = new FilteringSettings(RenderQueueRange.all, settings.LayerMask);
            m_RenderStateBlock = new RenderStateBlock(RenderStateMask.Nothing);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get();

            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            DrawingSettings drawSettings;
            drawSettings = CreateDrawingSettings(m_ShaderTagIdList, ref renderingData, SortingCriteria.CommonTransparent);
            context.DrawRenderers(renderingData.cullResults, ref drawSettings, ref m_FilteringSettings, ref m_RenderStateBlock);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    private GrabPass grabPass;
    private RenderPass renderPass;
    [SerializeField] private Settings settings;

    public override void Create()
    {
        grabPass = new GrabPass(settings);
        renderPass = new RenderPass(settings);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        grabPass.Setup(renderer.cameraColorTarget);

        renderer.EnqueuePass(grabPass);
        renderer.EnqueuePass(renderPass);
    }
}