Shader "Universal/OptimizedToon"
{
    Properties
    {
        [Header(Base Settings)]
        _BaseMap ("Base Texture", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
        
        [Header(Toon Shading)]
        _ToonSteps ("Toon Steps", Range(2, 10)) = 3
        _ToonSmoothness ("Toon Smoothness", Range(0.001, 0.1)) = 0.01
        _ShadowColor ("Shadow Color", Color) = (0.3, 0.3, 0.6, 1)
        _ShadowIntensity ("Shadow Intensity", Range(0, 1)) = 0.5
        
        [Header(Rim Lighting)]
        [Toggle(_RIM_LIGHTING)] _EnableRimLighting ("Enable Rim Lighting", Float) = 1
        _RimColor ("Rim Color", Color) = (1, 1, 1, 1)
        _RimPower ("Rim Power", Range(0.1, 10)) = 3
        _RimIntensity ("Rim Intensity", Range(0, 2)) = 1
        
        [Header(Outline)]
        [Toggle(_OUTLINE)] _EnableOutline ("Enable Outline", Float) = 1
        _OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineWidth ("Outline Width", Range(0.0001, 0.02)) = 0.003
        
        [Header(Specular)]
        [Toggle(_SPECULAR)] _EnableSpecular ("Enable Specular", Float) = 0
        _SpecularColor ("Specular Color", Color) = (1, 1, 1, 1)
        _SpecularSize ("Specular Size", Range(0.01, 1)) = 0.1
        _SpecularSmoothness ("Specular Smoothness", Range(0.001, 0.1)) = 0.01
        
        [Header(Advanced)]
        [Toggle(_RECEIVE_SHADOWS)] _ReceiveShadows ("Receive Shadows", Float) = 1
        [Toggle(_ALPHATEST_ON)] _AlphaClip ("Alpha Clipping", Float) = 0
        _Cutoff ("Alpha Cutoff", Range(0, 1)) = 0.5
        
        // Blending settings
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("Src Blend", Float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("Dst Blend", Float) = 0
        [Enum(Off,0,On,1)] _ZWrite("Z Write", Float) = 1
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("Z Test", Float) = 4
        [Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull", Float) = 2
    }

    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent" 
            "RenderPipeline" = "UniversalPipeline" 
            "Queue" = "Transparent"
        }
        
        LOD 300
        
        // Main Pass
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }
            
            Blend [_SrcBlend] [_DstBlend]
            ZWrite [_ZWrite]
            ZTest [_ZTest]
            Cull [_Cull]
            
            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            
            // Shader variants
            #pragma shader_feature_local _RIM_LIGHTING
            #pragma shader_feature_local _SPECULAR
            #pragma shader_feature_local _RECEIVE_SHADOWS
            #pragma shader_feature_local _ALPHATEST_ON
            
            // URP keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _SURFACE_TYPE_TRANSPARENT
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
            
            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            
            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                half4 _BaseColor;
                half _ToonSteps;
                half _ToonSmoothness;
                half4 _ShadowColor;
                half _ShadowIntensity;
                half4 _RimColor;
                half _RimPower;
                half _RimIntensity;
                half4 _SpecularColor;
                half _SpecularSize;
                half _SpecularSmoothness;
                half _Cutoff;
            CBUFFER_END
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float3 normalWS : TEXCOORD2;
                float3 viewDirWS : TEXCOORD3;
                float4 shadowCoord : TEXCOORD4;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };
            
            // Toon ramp function
            half ToonRamp(half dotProduct, half steps, half smoothness)
            {
                half ramp = dotProduct * 0.5 + 0.5;
                ramp = floor(ramp * steps) / steps;
                return smoothstep(0, smoothness, ramp);
            }
            
            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;
                
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS);
                
                output.positionCS = vertexInput.positionCS;
                output.positionWS = vertexInput.positionWS;
                output.normalWS = normalInput.normalWS;
                output.viewDirWS = GetWorldSpaceViewDir(vertexInput.positionWS);
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                
                #if defined(_RECEIVE_SHADOWS)
                    output.shadowCoord = GetShadowCoord(vertexInput);
                #endif
                
                return output;
            }
            
            half4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                
                // Sample base texture
                half4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv);
                half4 color = baseMap * _BaseColor;
                
                #if defined(_ALPHATEST_ON)
                    clip(color.a - _Cutoff);
                #endif
                
                // Get main light
                Light mainLight = GetMainLight();
                #if defined(_RECEIVE_SHADOWS)
                    mainLight = GetMainLight(input.shadowCoord);
                #endif
                
                // Normalize vectors
                float3 normalWS = normalize(input.normalWS);
                float3 lightDirWS = normalize(mainLight.direction);
                float3 viewDirWS = normalize(input.viewDirWS);
                
                // Calculate lighting
                half NdotL = dot(normalWS, lightDirWS);
                half toonRamp = ToonRamp(NdotL, _ToonSteps, _ToonSmoothness);
                
                // Apply toon shading
                half3 lighting = lerp(_ShadowColor.rgb * _ShadowIntensity, 1, toonRamp);
                lighting *= mainLight.color * mainLight.distanceAttenuation;
                
                color.rgb *= lighting;
                
                #if defined(_SPECULAR)
                    // Specular highlight
                    float3 halfDir = normalize(lightDirWS + viewDirWS);
                    half NdotH = saturate(dot(normalWS, halfDir));
                    half specular = pow(NdotH, 1.0 / _SpecularSize);
                    specular = smoothstep(0.5 - _SpecularSmoothness, 0.5 + _SpecularSmoothness, specular);
                    color.rgb += _SpecularColor.rgb * specular * mainLight.color;
                #endif
                
                #if defined(_RIM_LIGHTING)
                    // Rim lighting
                    half NdotV = 1.0 - saturate(dot(normalWS, viewDirWS));
                    half rim = pow(NdotV, _RimPower) * _RimIntensity;
                    color.rgb += _RimColor.rgb * rim;
                #endif
                
                // Additional lights (for mobile optimization, limit to important lights)
                #if defined(_ADDITIONAL_LIGHTS)
                    uint pixelLightCount = GetAdditionalLightsCount();
                    pixelLightCount = min(pixelLightCount, 4); // Limit for performance
                    
                    for (uint lightIndex = 0u; lightIndex < pixelLightCount; ++lightIndex)
                    {
                        Light light = GetAdditionalLight(lightIndex, input.positionWS);
                        half NdotL_add = dot(normalWS, light.direction);
                        half toonRamp_add = ToonRamp(NdotL_add, _ToonSteps, _ToonSmoothness);
                        
                        half3 additionalLighting = toonRamp_add * light.color * light.distanceAttenuation;
                        color.rgb += color.rgb * additionalLighting * 0.5; // Reduce intensity for additional lights
                    }
                #endif
                
                return color;
            }
            ENDHLSL
        }
        
        // Outline Pass - Fixed Method
        Pass
        {
            Name "Outline"
            Tags { "LightMode" = "SRPDefaultUnlit" }
            
            Cull Front
            ZWrite On
            ZTest LEqual
            
            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature_local _OUTLINE
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            CBUFFER_START(UnityPerMaterial)
                half4 _OutlineColor;
                half _OutlineWidth;
            CBUFFER_END
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };
            
            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;
                
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                
                #if defined(_OUTLINE)
                    // Smooth outline calculation
                    float3 normalOS = input.normalOS;
                    
                    // Use vertex color alpha for outline smoothing if available
                    float outlineScale = input.color.a > 0 ? input.color.a : 1.0;
                    
                    // Calculate position in view space for distance-based scaling
                    float4 positionCS = TransformObjectToHClip(input.positionOS.xyz);
                    float distance = positionCS.w;
                    
                    // Scale outline based on distance to maintain consistent thickness
                    float scaledWidth = _OutlineWidth * max(1.0, distance * 0.1);
                    
                    // Apply outline with smooth normal
                    float3 outlinePos = input.positionOS.xyz + normalize(normalOS) * scaledWidth * outlineScale;
                    output.positionCS = TransformObjectToHClip(outlinePos);
                #else
                    output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                #endif
                
                return output;
            }
            
            half4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                
                #if defined(_OUTLINE)
                    return _OutlineColor;
                #else
                    discard;
                    return 0;
                #endif
            }
            ENDHLSL
        }
        
        // Shadow Caster Pass
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }
            
            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull[_Cull]
            
            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment
            #pragma shader_feature_local _ALPHATEST_ON
            
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
            ENDHLSL
        }
        
        // Depth Only Pass
        Pass
        {
            Name "DepthOnly"
            Tags { "LightMode" = "DepthOnly" }
            
            ZWrite On
            ColorMask 0
            Cull[_Cull]
            
            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment
            #pragma shader_feature_local _ALPHATEST_ON
            
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
            ENDHLSL
        }
    }
    
    // Fallback for older hardware
    Fallback "Universal Render Pipeline/Simple Lit"
    CustomEditor "UnityEditor.Rendering.Universal.ShaderGUI.BaseShaderGUI"
}