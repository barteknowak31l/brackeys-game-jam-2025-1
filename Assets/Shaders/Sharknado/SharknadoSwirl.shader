Shader "Custom/TornadoEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Strength ("Swirl Strength", Range(0, 10)) = 5.0
        _Speed ("Rotation Speed", Range(0, 5)) = 1.0
        _Opacity ("Opacity", Range(0, 1)) = 1.0
        _Color ("Color Tint", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha // Ustawienie przezroczystości
        ZWrite Off // Wyłączenie zapisu głębi dla poprawnej przezroczystości

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Strength;
            float _Speed;
            float _Opacity;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.position = TransformObjectToHClip(v.position.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float2 SwirlUV(float2 uv, float strength, float time)
            {
                float2 center = float2(0.5, 0.5);
                float2 delta = uv - center;
                float distance = length(delta);
                
                float angle = strength * (0.5 - distance) + time;
                float sinAngle = sin(angle);
                float cosAngle = cos(angle);

                float2 rotatedUV;
                rotatedUV.x = cosAngle * delta.x - sinAngle * delta.y;
                rotatedUV.y = sinAngle * delta.x + cosAngle * delta.y;

                return center + rotatedUV;
            }

            half4 frag (v2f i) : SV_Target
            {
                float time = _Time.y * _Speed;
                float2 swirlUV = SwirlUV(i.uv, _Strength, time);

                half4 col = tex2D(_MainTex, swirlUV);
                col.rgb = lerp(col.rgb, _Color.rgb, _Color.a);

                float2 center = float2(0.5, 0.5);
                float alphaMask = saturate(length(i.uv - center) * 2.0);

                col.a *= alphaMask * _Opacity;

                return col;
            }
            ENDHLSL
        }
    }
}
