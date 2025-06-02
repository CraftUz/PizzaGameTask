Shader "Custom/ToonShader_WithShadowColor"
{
    Properties
    {
        _Color("Asosiy Rang", Color) = (1, 0.5, 0.5, 1) // Materialning asosiy rangi
        _ShadowColor("Soya Rang", Color) = (0, 0, 0.5, 1) // Soya uchun rang
        _RampSteps("Gölgelanish Bosqichlari", Range(2, 5)) = 3 // Soyalar bosqichi
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
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
            };

            float4 _Color; // Asosiy rang
            float4 _ShadowColor; // Soya rangi
            float _RampSteps; // Gölgelanish bosqichi

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal); // Normalni dunyo fazosiga o'tkazamiz
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Yorug'lik yo'nalishi (asosiy yorug'likni global qiymatidan olish)
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);

                // Yorug'lik va normal orasidagi burchak
                float NdotL = max(0, dot(i.worldNormal, lightDir));

                // Gölgelanish bosqichlarini hisoblash
                float stepValue = floor(NdotL * _RampSteps) / _RampSteps;

                // _ShadowColor va _Color o'rtasida aralashtirish
                fixed4 col = lerp(_ShadowColor, _Color, stepValue);

                return col;
            }
            ENDCG
        }
    }
}
