Shader "Custom/ToonShader"
{
    Properties
    {
        _Color("Asosiy Rang", Color) = (1,1,1,1)
        _MainTex("Textura", 2D) = "white" {}
        _Ramp("Nur Gölgelanishi", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 normal : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            sampler2D _MainTex;
            sampler2D _Ramp;
            float4 _Color;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
               float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
               float NdotL = saturate(dot(i.normal, lightDir)); // Normallashtirilgan nur yo‘nalishi
               float2 uv = float2(saturate(NdotL), 0); // UV koordinatalarini cheklaymiz
               float rampValue = tex2D(_Ramp, uv).r; // Ramp teksturasidan qiymat olamiz
               fixed4 col = tex2D(_MainTex, i.uv) * _Color; // Asosiy rang va tekstura
               col.rgb *= rampValue; // Ramp qiymati bilan ko‘paytirish
               return col;
            }
            ENDCG
        }
    }
}