﻿Shader "Lux URP/Toon & Outline HLSL"
{
    Properties
    {
        [HeaderHelpLuxURPToon_URL(6zvjnfuiskj1)]

        [Header(Surface Options)]
        [Space(8)]
        [Enum(UnityEngine.Rendering.CompareFunction)]
        _ZTest                      ("ZTest", Int) = 4
        [Enum(UnityEngine.Rendering.CullMode)]
        _Cull                       ("Culling", Float) = 2
        [Toggle(_ALPHATEST_ON)]
        _AlphaClip                  ("Alpha Clipping", Float) = 0.0
        [LuxURPHelpDrawer]
        _Help ("Enabling Alpha Clipping needs you to enable and assign the Albedo (RGB) Alpha (A) Map as well.", Float) = 0.0
        _Cutoff                     ("     Threshold", Range(0.0, 1.0)) = 0.5
        [Enum(Off,0,On,1)]_Coverage ("     Alpha To Coverage", Float) = 0
        [Enum(All,15,Depth,0)]
        _ColorMask ("Color Mask", Float) = 15

        [Space(5)]
        [Toggle(_SSAO_ENABLED)]
        _ReceiveSSAO                ("Receive SSAO", Float) = 1.0
        
        [Header(Toon Lighting)]
        [Space(8)]
        [IntRange] _Steps           ("Steps", Range(1, 8)) = 1
        _DiffuseFallOff             ("Diffuse Falloff", Range(0, 1)) = 0.01
        _DiffuseStep                ("Diffuse Step", Range(-1, 1)) = 0
        [Space(5)]
        [KeywordEnum(Off, SmoothSampling, PointSampling)] _Ramp ("Ramp Mode", Float) = 0
        [NoScaleOffset]
        _GradientMap                ("     Ramp", 2D) = "white" {}
        _OcclusionStrength          ("Occlusion", Range(0.0, 1.0)) = 1

        [Header(Advanced Toon Lighting)]
        [Space(8)]
        //[Toggle(_COLORIZEMAIN)]
        _ColorizedShadowsMain       ("Colorize Main Light", Range(0, 1)) = 0.0
        //[Toggle(_COLORIZEADD)]
        _ColorizedShadowsAdd        ("Colorize Add Lights", Range(0, 1)) = 0.0
        _LightColorContribution     ("Light Color Contribution", Range(0, 1)) = 1
        _AddLightFallOff            ("Light Falloff", Range(0.0001, 1)) = 1

        [Header(Specular Toon Lighting)]
        [Space(8)]
        [ToggleOff]
        _SpecularHighlights         ("Enable Specular Highlights", Float) = 1.0
        [Toggle(_ANISOTROPIC)]
        _Anisotropic                ("     Anisotropic Specular", Float) = 0.0
        _Anisotropy                 ("          Anisotropy", Range(-1.0, 1.0)) = 0.0
        [Space(5)]
        [Toggle]
        _EnergyConservation         ("     EnergyConservation", Float) = 1
        [HDR] _SpecColor            ("     Specular", Color) = (0.2, 0.2, 0.2)
        [HDR] _SpecColor2nd         ("     Secondary Specular", Color) = (0.4, 0.4, 0.4)
        _Smoothness                 ("     Smoothness", Range(0.0, 1.0)) = 0.5
        _SpecularStep               ("     Specular Step", Range(0.3, 0.5)) = 0.45
        _SpecularFallOff            ("     Specular Falloff", Range(0, 1)) = 0.01
        

        [Header(Toon Rim)]
        [Space(8)]
        [Toggle(_TOONRIM)]
        _EnableToonRim              ("Enable Toon Rim", Float) = 0.0
        [HDR] _ToonRimColor         ("     Rim Color", Color) = (1, 1, 1, 1)
        _ToonRimPower               ("     Rim Power", Range(0, 1)) = 0.5
        _ToonRimFallOff             ("     Rim Falloff", Range(0, 1)) = 1
        _ToonRimAttenuation         ("     Rim Attenuation", Range(0, 1)) = 0


        [Header(Toon Shadows)]
        [Space(8)]
        [ToggleOff(_RECEIVE_SHADOWS_OFF)]
        _ReceiveShadows             ("Receive Shadows", Float) = 1.0
        _ShadowOffset               ("     Shadow Offset", Float) = 1.0
        _ShadowFallOff              ("     Shadow Falloff", Range(0.5, 1)) = 0.75
        _ShadoBiasDirectional       ("     Shadow Bias Directional", Range(0, 1)) = 0
        _ShadowBiasAdditional       ("     Shadow Bias Additional", Range(0, 1)) = 0
        

        [Header(Toon Outline)]
        [Space(8)]
        _OutlineColor               ("Color (RGB) Alpha (A)", Color) = (0,0,0,1)
        _Border                     ("Width", Float) = 10
        [Toggle(_COMPENSATESCALE)]
        _CompensateScale            ("Compensate Object Scale", Float) = 0
        [Toggle(_OUTLINEINSCREENSPACE)]
        _OutlineInScreenSpace       ("Calculate width in Screen Space", Float) = 0
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTestOutline ("ZTest Outline", Int) = 4
        [Enum(UnityEngine.Rendering.CullMode)]
        _CullOutline                ("Culling Outline", Float) = 1


        [Header(Surface Inputs)]
        [Space(8)]
        [MainColor]
        _BaseColor                  ("Color (RGB) Alpha (A)", Color) = (1,1,1,1)
        _ShadedBaseColor            ("Shaded Color (RGB)", Color) = (0,0,0,1)

        [Space(5)]
        [KeywordEnum(Off, One, Two)] _TexMode ("TextureMode", Float) = 0
        [MainTexture]
        _BaseMap                    ("Albedo (RGB) Alpha (A)", 2D) = "white" {}
        [NoScaleOffset]
        _ShadedBaseMap              ("Shaded Albedo (RGB)", 2D) = "white" {}

        [Space(5)]
        [Toggle(_MASKMAP)]
        _EnableMaskMap              ("Enable Mask Map", Float) = 0.0
        [NoScaleOffset]
        _MaskMap                    ("     Emission (R) Specular (G) Occlusion (B) Smoothness (A)", 2D) = "white" {}
        [HDR] _EmissionColor        ("     Emission Color", Color) = (0,0,0,0)

        [Space(5)]
        [Toggle(_NORMALMAP)]
        _ApplyNormal                ("Enable Normal Map", Float) = 0.0
        [NoScaleOffset]
        _BumpMap                    ("     Normal Map", 2D) = "bump" {}
        _BumpScale                  ("     Normal Scale", Float) = 1.0

        
    //  Lux URP standards 
        [Header(Rim Lighting)]
        [Space(8)]
        [Toggle(_RIMLIGHTING)]
        _Rim                        ("Enable Rim Lighting", Float) = 0
        [HDR] _RimColor             ("Rim Color", Color) = (0.5,0.5,0.5,1)
        _RimPower                   ("Rim Power", Float) = 2
        _RimFrequency               ("Rim Frequency", Float) = 0
        _RimMinPower                ("     Rim Min Power", Float) = 1
        _RimPerPositionFrequency    ("     Rim Per Position Frequency", Range(0.0, 1.0)) = 1

        [Header(Stencil)]
        [Space(8)]
        [IntRange] _Stencil         ("Stencil Reference", Range (0, 255)) = 0
        [IntRange] _ReadMask        ("     Read Mask", Range (0, 255)) = 255
        [IntRange] _WriteMask       ("     Write Mask", Range (0, 255)) = 255
        [Enum(UnityEngine.Rendering.CompareFunction)]
        _StencilComp                ("Stencil Comparison", Int) = 8     // always – terrain should be the first thing being rendered anyway
        [Enum(UnityEngine.Rendering.StencilOp)]
        _StencilOp                  ("Stencil Operation", Int) = 0      // 0 = keep, 2 = replace
        [Enum(UnityEngine.Rendering.StencilOp)]
        _StencilFail                ("Stencil Fail Op", Int) = 0        // 0 = keep
        [Enum(UnityEngine.Rendering.StencilOp)] 
        _StencilZFail               ("Stencil ZFail Op", Int) = 0       // 0 = keep

        [Header(Advanced)]
        [Space(8)]
        [ToggleOff]
        _EnvironmentReflections     ("Environment Reflections", Float) = 0.0
        
        [Header(Render Queue)]
        [Space(8)]
        [IntRange] _QueueOffset     ("Queue Offset", Range(-50, 50)) = 0


    //  Needed by the inspector
        [HideInInspector] _Culling  ("Culling", Float) = 0.0
        [HideInInspector] _AlphaFromMaskMap  ("AlphaFromMaskMap", Float) = 1.0

    //  Lightmapper and outline selection shader need _MainTex, _Color and _Cutoff
        [HideInInspector] _MainTex  ("Albedo", 2D) = "white" {}
        [HideInInspector] _Color    ("Color", Color) = (1,1,1,1)

        [HideInInspector]
        _SrcBlend                   ("SrcBlend", Float) = 1.0
        [HideInInspector]
        _DstBlend                   ("DstBlend", Float) = 0.0
        [HideInInspector] 
        _ZWrite                     ("ZWrite", Float) = 1.0
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Opaque"
            "Queue" = "Geometry"
        }
        LOD 100

//  /////////////////////////////////////////////////////
//  Outline Pass

        Pass
        {
            Name "Outline"

//  Here we have to fool URP < 8.0: We want the outline to render AFTER the regular shaded pass.
//  This worked fine in URP 7.4.1 but 8.x and above would draw the outine first...
//  So we tag the outline pass as "LightMode" = "UniversalForward" which makes unity draw it after our "regular" pass.
            
            Tags{"LightMode" = "UniversalForward"}
            
            //Tags{"LightMode" = "Outline"} // Needed to disable pass but then it does not get rendered...
            Blend SrcAlpha OneMinusSrcAlpha
            Cull [_CullOutline]
            ZWrite On
            ZTest [_ZTestOutline]
            AlphaToMask [_Coverage]

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard SRP library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma shader_feature_local _ALPHATEST_ON

            #pragma shader_feature_local _COMPENSATESCALE
            #pragma shader_feature_local _OUTLINEINSCREENSPACE

            #pragma multi_compile_fog
            #pragma multi_compile_instancing

            #pragma vertex OutlineVertex
            #pragma fragment OutlineFrag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"

        //  Include base inputs and all other needed "base" includes
            #include "Includes/Lux URP Toon Inputs.hlsl"

            //#if defined(_ALPHATEST_ON)
                //TEXTURE2D(_Basemap); SAMPLER(sampler_Basemap);
            //#endif

            struct VertexInputOutline
            {
                float4 vertex : POSITION;
                #if defined(_ALPHATEST_ON)
                    float2 texcoord : TEXCOORD0;
                    float2 uv : TEXCOORD1;
                #else
                    float3 normal : NORMAL;  
                #endif 
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct VertexOutputOutline
            {
                float4 positionCS : POSITION;
                half fogCoord : TEXCOORD0;
                #if defined(_ALPHATEST_ON)
                    float2 uv : TEXCOORD1;
                #endif    
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            VertexOutputOutline OutlineVertex(VertexInputOutline input)
            {
                VertexOutputOutline output = (VertexOutputOutline)0;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                #if !defined(_ALPHATEST_ON)
                //  Extrude
                    #if !defined(_OUTLINEINSCREENSPACE)
                        #if defined(_COMPENSATESCALE)
                            float3 scale;
                            scale.x = length(float3(UNITY_MATRIX_M[0].x, UNITY_MATRIX_M[1].x, UNITY_MATRIX_M[2].x));
                            scale.y = length(float3(UNITY_MATRIX_M[0].y, UNITY_MATRIX_M[1].y, UNITY_MATRIX_M[2].y));
                            scale.z = length(float3(UNITY_MATRIX_M[0].z, UNITY_MATRIX_M[1].z, UNITY_MATRIX_M[2].z));
                        #endif
                        input.vertex.xyz += input.normal * 0.001 * _Border
                        #if defined(_COMPENSATESCALE) 
                            / scale
                        #endif
                        ;
                    #endif
                    output.positionCS = TransformObjectToHClip(input.vertex.xyz);
                //  Extrude
                    #if defined(_OUTLINEINSCREENSPACE)
                        if (_Border > 0.0h) {
                            //float3 normal = mul(UNITY_MATRIX_MVP, float4(v.normal, 0)).xyz; // to clip space
                            float3 normal = mul(GetWorldToHClipMatrix(), mul(GetObjectToWorldMatrix(), float4(v.normal, 0.0))).xyz;
                            float2 offset = normalize(normal.xy);
                            float2 ndc = _ScreenParams.xy * 0.5;
                            output.positionCS.xy += ((offset * _Border) / ndc * output.positionCS.w);
                        }
                    #endif
                #else
                    output.uv = TRANSFORM_TEX(input.texcoord, _BaseMap);
                    output.positionCS = TransformObjectToHClip(input.vertex.xyz);
                #endif

                output.fogCoord = ComputeFogFactor(output.positionCS.z);
                return output;
            }

            //  Helper
            inline float2 shufflefast (float2 offset, float2 shift) {
                return offset * shift;
            }

            half4 OutlineFrag (VertexOutputOutline input ) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                #if defined(_ALPHATEST_ON)
                    float2 uv = input.uv;

                    float2 offset = float2(1,1);
                    #if defined(_OUTLINEINSCREENSPACE)
                        float2 shift = fwidth(uv) * _Border * 0.5f;
                    #else
                        float2 shift = _Border.xx * 0.5 * _BaseMap_TexelSize.xy;
                    #endif

                    float2 sampleCoord = uv + shufflefast(offset, shift); 
                    half shuffleAlpha = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, sampleCoord).a;

                    offset = float2(-1,1);
                    sampleCoord = uv + shufflefast(offset, shift);
                    shuffleAlpha += SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, sampleCoord).a;

                    offset = float2(1,-1);
                    sampleCoord = uv + shufflefast(offset, shift);
                    shuffleAlpha += SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, sampleCoord).a;

                    offset = float2(-1,-1);
                    sampleCoord = uv + shufflefast(offset, shift);
                    shuffleAlpha += SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, sampleCoord).a;

                    //shuffleAlpha = saturate(shuffleAlpha); // not needed
                    //shuffleAlpha *= 0.25;                  // bad!

                //  Mask inner parts - does not work properly with different _Cutoff values?!
                //  So we go with ZTest Less 
                    //shuffleAlpha = shuffleAlpha * ( 1 - step(_Cutoff, innerAlpha) );
                    //shuffleAlpha = lerp(shuffleAlpha, 0, step(_Cutoff, innerAlpha) );
                //  Apply clip
                    clip(shuffleAlpha - _Cutoff);
                #endif
                half4 color = _OutlineColor;
                color.rgb = MixFog(color.rgb, input.fogCoord);
                return color;
            }
            ENDHLSL

        }

//  /////////////////////////////////////////////////////
//  Regular Passes

        Pass
        {
            Name "ForwardLit"

//  Outline pass uses "LightMode" = "UniversalForward" to fool URP > 8.x
            //Tags{"LightMode" = "UniversalForward"}

            Stencil {
                Ref   [_Stencil]
                ReadMask [_ReadMask]
                WriteMask [_WriteMask]
                Comp  [_StencilComp]
                Pass  [_StencilOp]
                Fail  [_StencilFail]
                ZFail [_StencilZFail]
            }
            
            Blend [_SrcBlend][_DstBlend]
            ZWrite [_ZWrite]
            ZTest [_ZTest]
            Cull [_Cull]
            AlphaToMask [_Coverage]
//  Experimental!
            ColorMask [_ColorMask]

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard SRP library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x

        //  Shader target needs to be 3.0 due to tex2Dlod in the vertex shader or VFACE
            #pragma target 3.0

            // -------------------------------------
            // Material Keywords
            #define _SPECULAR_SETUP 1

            #pragma shader_feature_local _ALPHATEST_ON
            #pragma shader_feature_local_fragment _COLORIZEMAIN
            #pragma shader_feature_local_fragment _COLORIZEADD
            #pragma shader_feature_local_fragment _TOONRIM
            //#pragma shader_feature_local GRADIENT_ON
            #pragma shader_feature_local _ _RAMP_SMOOTHSAMPLING _RAMP_POINTSAMPLING
            #pragma shader_feature_local _ _TEXMODE_ONE _TEXMODE_TWO

            #pragma shader_feature_local_fragment _SSAO_ENABLED

            #pragma shader_feature_local _MASKMAP

            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local_fragment _RIMLIGHTING

            #pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
            #pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF
            #pragma shader_feature_local _RECEIVE_SHADOWS_OFF

            #pragma shader_feature_local _ANISOTROPIC       // Also affects vertex shader! (vertex input)

            // -------------------------------------
            // Universal Pipeline keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
            #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
            #pragma multi_compile _ SHADOWS_SHADOWMASK

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile_fog

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing

        //  Include base inputs and all other needed "base" includes
            #include "Includes/Lux URP Toon Inputs.hlsl"

            #pragma vertex LitPassVertex
            #pragma fragment LitPassFragment

        //--------------------------------------
        //  Vertex shader


            VertexOutput LitPassVertex(VertexInput input)
            {
                VertexOutput output = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                VertexPositionInputs vertexInput; // 
                vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                
                #if defined(_NORMALMAP) || (defined(_ANISOTROPIC) && !defined(_SPECULARHIGHLIGHTS_OFF))
                    VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);
                #else
                    VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, float4(0,0,0,0));
                #endif

                float3 viewDirWS = GetCameraPositionWS() - vertexInput.positionWS;
                half3 vertexLight = VertexLighting(vertexInput.positionWS, normalInput.normalWS);
                half fogFactor = ComputeFogFactor(vertexInput.positionCS.z);

                #if defined(_TEXMODE_ONE) || defined(_TEXMODE_TWO) || defined(_TEXMODE_TWO) || defined(_NORMALMAP) || defined(_MASKMAP)
                    output.uv = TRANSFORM_TEX(input.texcoord, _BaseMap);
                #endif

                output.normalWS = normalInput.normalWS;
                output.viewDirWS = viewDirWS;

                #if defined (_NORMALMAP) || (defined(_ANISOTROPIC) && !defined(_SPECULARHIGHLIGHTS_OFF))
                    float sign = input.tangentOS.w * GetOddNegativeScale();
                    output.tangentWS = float4(normalInput.tangentWS.xyz, sign);
                #endif

                OUTPUT_LIGHTMAP_UV(input.lightmapUV, unity_LightmapST, output.lightmapUV);
                OUTPUT_SH(output.normalWS.xyz, output.vertexSH);
                
                output.fogFactorAndVertexLight = half4(fogFactor, vertexLight);

                #if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
                    output.positionWS = vertexInput.positionWS;
                #endif

                #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                    output.shadowCoord = GetShadowCoord(vertexInput);
                #endif
                output.positionCS = vertexInput.positionCS;

                return output;
            }

        //--------------------------------------
        //  Fragment shader and functions

            inline void InitializeSurfaceData(
                float2 uv,
                out SurfaceDescription outSurfaceData)
            {
                #if defined(_TEXMODE_ONE) || defined(_TEXMODE_TWO)
                    half4 albedoAlpha = SampleAlbedoAlpha(uv.xy, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
                    albedoAlpha *= _BaseColor;
                #else
                    half4 albedoAlpha = _BaseColor;
                #endif
                
                #if defined(_ALPHATEST_ON) && (defined(_TEXMODE_ONE) || defined(_TEXMODE_TWO))
                    outSurfaceData.alpha = Alpha(albedoAlpha.a, 1, _Cutoff);
                #else
                    outSurfaceData.alpha = albedoAlpha.a;
                #endif

                #if defined(_TEXMODE_TWO)
                    half3 albedoShaded = SampleAlbedoAlpha(uv, TEXTURE2D_ARGS(_ShadedBaseMap, sampler_BaseMap)).rgb;
                #else
                    #if defined(_TEXMODE_ONE)
                        half3 albedoShaded = albedoAlpha.rgb;
                    #else
                        half3 albedoShaded = half3(1,1,1);
                    #endif
                #endif

                #if defined(_MASKMAP)
                    half4 maskSample = SAMPLE_TEXTURE2D(_MaskMap, sampler_MaskMap, uv);
                    outSurfaceData.occlusion = lerp(1.0h, maskSample.b, _OcclusionStrength);
                    outSurfaceData.smoothness = maskSample.a * _Smoothness;
                    outSurfaceData.specular = lerp(_SpecColor2nd.rgb, _SpecColor.rgb, maskSample.ggg);
                    outSurfaceData.emission = maskSample.r * _EmissionColor.rgb;
                #else
                    outSurfaceData.occlusion = _OcclusionStrength;
                    outSurfaceData.smoothness = _Smoothness;
                    outSurfaceData.specular = _SpecColor.rgb;
                    outSurfaceData.emission = 0;
                #endif 

                outSurfaceData.albedo = albedoAlpha.rgb;
                outSurfaceData.albedoShaded = albedoShaded * _ShadedBaseColor.rgb;
                outSurfaceData.metallic = 0;
                
            //  Normal Map
                #if defined (_NORMALMAP)
                    outSurfaceData.normalTS = SampleNormal(uv.xy, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
                #else
                    outSurfaceData.normalTS = half3(0,0,1);
                #endif
            }

            void InitializeInputData(VertexOutput input, half3 normalTS, half facing, out InputData inputData)
            {
                inputData = (InputData)0;
                #if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
                    inputData.positionWS = input.positionWS;
                #endif

                half3 viewDirWS = SafeNormalize(input.viewDirWS);
                #if defined(_NORMALMAP)
                    normalTS.z *= facing;
                    float sgn = input.tangentWS.w;      // should be either +1 or -1
                    float3 bitangent = sgn * cross(input.normalWS.xyz, input.tangentWS.xyz);
                    inputData.normalWS = TransformTangentToWorld(normalTS, half3x3(input.tangentWS.xyz, bitangent.xyz, input.normalWS.xyz));
                #else
                    inputData.normalWS = input.normalWS * facing;
                #endif

            //  Not normalized normals cause uggly specular highlights on mobile. So we always normalize.
                #if !defined(_SPECULARHIGHLIGHTS_OFF)
                    inputData.normalWS = normalize(inputData.normalWS);
                #else
                    inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
                #endif
                inputData.viewDirectionWS = viewDirWS;
                
                #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                    inputData.shadowCoord = input.shadowCoord;
                #elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
                    inputData.shadowCoord = TransformWorldToShadowCoord(inputData.positionWS);
                #else
                    inputData.shadowCoord = float4(0, 0, 0, 0);
                #endif
                inputData.fogCoord = input.fogFactorAndVertexLight.x;
                inputData.vertexLighting = input.fogFactorAndVertexLight.yzw;
                inputData.bakedGI = SAMPLE_GI(input.lightmapUV, input.vertexSH, inputData.normalWS);
            
                //inputData.normalizedScreenSpaceUV = input.positionCS.xy;
                inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(input.positionCS);
                inputData.shadowMask = SAMPLE_SHADOWMASK(input.lightmapUV);
            }

            half4 LitPassFragment(VertexOutput input, half facing : VFACE) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

            //  Get the surface description
                SurfaceDescription surfaceData;
                #if defined(_TEXMODE_ONE) || defined(_TEXMODE_TWO) || defined(_TEXMODE_TWO) || defined(_NORMALMAP) || defined(_MASKMAP)
                    InitializeSurfaceData(input.uv, surfaceData);
                #else
                    InitializeSurfaceData(float2(0,0), surfaceData);
                #endif

            //  Prepare surface data (like bring normal into world space and get missing inputs like gi)
                InputData inputData;
                InitializeInputData(input, surfaceData.normalTS, facing, inputData);

                #if defined(_RIMLIGHTING)
                    half rim = saturate(1.0h - saturate( dot(inputData.normalWS, inputData.viewDirectionWS) ) );
                    half power = _RimPower;
                    UNITY_BRANCH if(_RimFrequency > 0 ) {
                        half perPosition = lerp(0.0h, 1.0h, dot(1.0h, frac(UNITY_MATRIX_M._m03_m13_m23) * 2.0h - 1.0h ) * _RimPerPositionFrequency ) * 3.1416h;
                        power = lerp(power, _RimMinPower, (1.0h + sin(_Time.y * _RimFrequency + perPosition) ) * 0.5h );
                    }
                    surfaceData.emission += pow(rim, power) * _RimColor.rgb * _RimColor.a;
                #endif

            //  Apply lighting
                half4 color = LuxURPToonFragmentPBR(
                    inputData,

                    #if defined(_ANISOTROPIC) && !defined(_SPECULARHIGHLIGHTS_OFF)
                        input.tangentWS,
                        _Anisotropy,
                    #endif

                    surfaceData.albedo,
                    surfaceData.albedoShaded,

                    surfaceData.metallic, 
                    surfaceData.specular,

                    _Steps,
                    _DiffuseStep,
                    _DiffuseFallOff,
                    
                    _EnergyConservation,
                    _SpecularStep,
                    _SpecularFallOff,

                    _ColorizedShadowsMain,
                    _ColorizedShadowsAdd,
                    _LightColorContribution,
                    _AddLightFallOff,
                    
                    _ShadowFallOff,
                    _ShadoBiasDirectional,
                    _ShadowBiasAdditional,

                    _ToonRimColor.rgb,
                    _ToonRimPower,
                    _ToonRimFallOff,
                    _ToonRimAttenuation,

                    surfaceData.smoothness, 
                    surfaceData.occlusion, 
                    surfaceData.emission, 
                    surfaceData.alpha
                );    
            //  Add fog
                color.rgb = MixFog(color.rgb, inputData.fogCoord);
                return color;
            }

            ENDHLSL
        }


    //  Shadows -----------------------------------------------------
        
        Pass
        {
            Name "ShadowCaster"
            Tags{"LightMode" = "ShadowCaster"}

            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull[_Cull]

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local _ALPHATEST_ON
            #pragma shader_feature_local _ _TEXMODE_ONE _TEXMODE_TWO


            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing

            // -------------------------------------
            // Universal Pipeline keywords
            // This is used during shadow map generation to differentiate between directional and punctual light shadows, as they use different formulas to apply Normal Bias
            #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW


            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

        //  Include base inputs and all other needed "base" includes
            #include "Includes/Lux URP Toon Inputs.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
            
        //  Shadow caster specific input
            float3 _LightDirection;
            float3 _LightPosition;

            VertexOutput ShadowPassVertex(VertexInput input)
            {
                VertexOutput output = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);

                #if defined(_ALPHATEST_ON) && (defined(_TEXMODE_ONE) || defined(_TEXMODE_TWO))
                    output.uv.xy = TRANSFORM_TEX(input.texcoord, _BaseMap);
                #endif

                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldDir(input.normalOS);

                #if _CASTING_PUNCTUAL_LIGHT_SHADOW
                    float3 lightDirectionWS = normalize(_LightPosition - positionWS);
                #else
                    float3 lightDirectionWS = _LightDirection;
                #endif

                output.positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS * _ShadowOffset, lightDirectionWS));
                #if UNITY_REVERSED_Z
                    output.positionCS.z = min(output.positionCS.z, UNITY_NEAR_CLIP_VALUE);
                #else
                    output.positionCS.z = max(output.positionCS.z, UNITY_NEAR_CLIP_VALUE);
                #endif
                return output;
            }

            half4 ShadowPassFragment(VertexOutput input) : SV_TARGET
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                #if defined(_ALPHATEST_ON) && (defined(_TEXMODE_ONE) || defined(_TEXMODE_TWO))
                    half mask = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv).a;
                    clip (mask - _Cutoff);
                #endif
                return 0;
            }
            ENDHLSL
        }

    //  Depth -----------------------------------------------------

        Pass
        {
            Tags{"LightMode" = "DepthOnly"}

            ZWrite On
            ColorMask 0
            Cull[_Cull]

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local _ALPHATEST_ON
            #pragma shader_feature_local _ _TEXMODE_ONE _TEXMODE_TWO

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            
            #define DEPTHONLYPASS
            #include "Includes/Lux URP Toon Inputs.hlsl"

            VertexOutput DepthOnlyVertex(VertexInput input)
            {
                VertexOutput output = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                #if defined(_ALPHATEST_ON) && (defined(_TEXMODE_ONE) || defined(_TEXMODE_TWO))
                    output.uv.xy = TRANSFORM_TEX(input.texcoord, _BaseMap);
                #endif

                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                return output;
            }

            half4 DepthOnlyFragment(VertexOutput input) : SV_TARGET
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                #if defined(_ALPHATEST_ON) && (defined(_TEXMODE_ONE) || defined(_TEXMODE_TWO))
                    half mask = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv.xy).a;
                    clip (mask - _Cutoff);
                #endif

                return 0;
            }

            ENDHLSL
        }

    //  Depth Normals -----------------------------------------------------
        
        Pass
        {
            Name "DepthNormals"
            Tags{"LightMode" = "DepthNormals"}

            ZWrite On
            Cull[_Cull]

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard SRP library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma vertex DepthNormalsVertex
            #pragma fragment DepthNormalsFragment

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local _ALPHATEST_ON
            #pragma shader_feature_local _ _TEXMODE_ONE _TEXMODE_TWO

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            // #pragma multi_compile _ DOTS_INSTANCING_ON // needs shader target 4.5

            //#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Includes/Lux URP Toon Inputs.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthNormalsPass.hlsl"
            ENDHLSL
        }

    //  Meta -----------------------------------------------------
        
        Pass
        {
            Tags{"LightMode" = "Meta"}

            Cull Off

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles

            #pragma vertex UniversalVertexMeta
            #pragma fragment UniversalFragmentMeta

            #define _SPECULAR_SETUP

        //  First include all our custom stuff
            #include "Includes/Lux URP Toon Inputs.hlsl"

        //--------------------------------------
        //  Fragment shader and functions

            inline void InitializeStandardLitSurfaceData(float2 uv, out SurfaceData outSurfaceData)
            {
                half4 albedoAlpha = SampleAlbedoAlpha(uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
                outSurfaceData.alpha = Alpha(albedoAlpha.a, half4(1.0h, 1.0h, 1.0h, 1.0h), _Cutoff);
                outSurfaceData.albedo = albedoAlpha.rgb * _BaseColor.rgb;
                outSurfaceData.metallic = 0;
                outSurfaceData.specular = _SpecColor.rgb;
                outSurfaceData.smoothness = _Smoothness;
                outSurfaceData.normalTS = half3(0,0,1);
                outSurfaceData.occlusion = 1;
                outSurfaceData.emission = 0;

            //  UPR 10+
                //#if VERSION_GREATER_EQUAL(10, 0)
                    outSurfaceData.clearCoatMask = 0;
                    outSurfaceData.clearCoatSmoothness = 0;
                //#endif
            }

        //  Finally include the meta pass related stuff  
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitMetaPass.hlsl"

            ENDHLSL
        }

    //  End Passes -----------------------------------------------------
    
    }
    FallBack "Hidden/InternalErrorShader"
    CustomEditor "LuxURPUniversalCustomShaderGUI"
}
