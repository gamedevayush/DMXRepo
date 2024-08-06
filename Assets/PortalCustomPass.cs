using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

class PortalCustomPass : CustomPass
{
    protected override void Execute(CustomPassContext ctx)
    {
        CoreUtils.SetRenderTarget(ctx.cmd, ctx.cameraColorBuffer, ctx.cameraDepthBuffer, ClearFlag.None, Color.clear);

        // Set stencil test to only render where the stencil buffer value is 1
        var material = CoreUtils.CreateEngineMaterial("Shader Graphs/MainShader"); // Use a default HDRP material for copying

        // Set stencil parameters
        material.SetInt("_StencilComp", (int)CompareFunction.Equal);
        material.SetInt("_Stencil", 1);

        // Full-screen pass to apply the portal effect
        ctx.cmd.DrawProcedural(Matrix4x4.identity, material, 0, MeshTopology.Triangles, 3, 1);
    }
}
