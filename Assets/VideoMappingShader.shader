Shader "Custom/VideoMappingShaderWithEmission"
{
    Properties
    {
        _MainTex ("Video Texture", 2D) = "white" {}
        _UVOffset ("UV Offset", Vector) = (0, 0, 1, 1) // (X Offset, Y Offset, X Scale, Y Scale)
        _EmissionTex ("Emission Texture", 2D) = "black" {} // Emission texture
        _EmissionColor ("Emission Color", Color) = (1, 1, 1, 1) // Emission color
        _EmissionIntensity ("Emission Intensity", Range(0, 10)) = 1 // Intensity of the emission
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _EmissionTex; // Emission texture
            float4 _UVOffset; // (X Offset, Y Offset, X Scale, Y Scale)
            float4 _EmissionColor; // Emission color
            float _EmissionIntensity; // Emission intensity

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Apply custom UV transformation
                o.uv.x = v.uv.x * _UVOffset.z + _UVOffset.x;
                o.uv.y = v.uv.y * _UVOffset.w + _UVOffset.y;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Sample the main texture
                fixed4 mainTexColor = tex2D(_MainTex, i.uv);
                
                // Sample the emission texture
                fixed4 emissionTexColor = tex2D(_EmissionTex, i.uv) * _EmissionColor * _EmissionIntensity;

                // Combine the main texture color with the emission color
                return mainTexColor + emissionTexColor;
            }
            ENDCG
        }
    }
}