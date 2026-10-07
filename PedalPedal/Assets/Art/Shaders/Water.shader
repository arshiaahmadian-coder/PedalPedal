Shader "Custom/RiverGradient"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}

        _DarkColor ("Center Color", Color) = (0.02, 0.18, 0.40, 1)
        _LightColor ("Edge Color", Color) = (0.20, 0.75, 1.0, 1)

        _GradientPower ("Gradient", Range(0.1, 5.0)) = 1.5
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "RenderPipeline"="UniversalPipeline"
            "CanUseSpriteAtlas"="True"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            Tags { "LightMode"="Universal2D" }

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float3 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)

            float4 _DarkColor;
            float4 _LightColor;
            float _GradientPower;

            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = IN.uv;
                OUT.color = IN.color;

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 sprite = SAMPLE_TEXTURE2D(
                    _MainTex,
                    sampler_MainTex,
                    IN.uv
                );

                // Distance from the vertical center of the Fill UV.
                float centerDistance = abs(IN.uv.y - 0.5) * 2.0;

                // Center = 0, edges = 1.
                float gradient = pow(
                    saturate(centerDistance),
                    _GradientPower
                );

                half3 color = lerp(
                    _DarkColor.rgb,
                    _LightColor.rgb,
                    gradient
                );

                return half4(
                    color * IN.color.rgb,
                    sprite.a * IN.color.a
                );
            }

            ENDHLSL
        }
    }
}