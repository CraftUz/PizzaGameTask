Shader "Custom/ToonShaderWebGL"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _ShadeColor ("Shade Color", Color) = (0.5,0.5,0.5,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 normal : TEXCOORD0;
            };

            fixed4 _Color;
            fixed4 _ShadeColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.normal = normalize(mul(v.normal, (float3x3)unity_WorldToObject));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Yorug‘lik yo‘nalishi (eng oddiy usulda hisoblaymiz)
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);

                // Yorug‘likni hisoblash
                float ndotl = dot(i.normal, lightDir);

                // Yorug‘lik kuchini qalin qadam orqali sozlaymiz
                fixed4 color = ndotl > 0.5 ? _Color : _ShadeColor;

                return color;
            }
            ENDCG
        }
    }
}
