Shader "Custom/BoatWakeCartoon"
{
    Properties
    {
        _MainColor ("Wake Color", Color) = (0.85, 1.0, 0.98, 0.75)
        _HighlightColor ("Highlight Color", Color) = (1.0, 1.0, 1.0, 0.8)

        _Opacity ("Opacity", Range(0,1)) = 0.65
        _FadePower ("Length Fade", Range(0.2,5)) = 1.6
        _EdgeSoftness ("Edge Softness", Range(0.01,1)) = 0.35

        _WaveStrength ("Wave Strength", Range(0,0.2)) = 0.035
        _WaveFrequency ("Wave Frequency", Range(1,30)) = 10
        _WaveSpeed ("Wave Speed", Range(0,10)) = 2.0

        _HighlightStrength ("Highlight Strength", Range(0,2)) = 0.7
        _HighlightWidth ("Highlight Width", Range(0.01,1)) = 0.18
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
            "RenderPipeline"="UniversalPipeline"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            Name "BoatWake"

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            CBUFFER_START(UnityPerMaterial)

                float4 _MainColor;
                float4 _HighlightColor;

                float _Opacity;
                float _FadePower;
                float _EdgeSoftness;

                float _WaveStrength;
                float _WaveFrequency;
                float _WaveSpeed;

                float _HighlightStrength;
                float _HighlightWidth;

            CBUFFER_END


            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                float3 position = IN.positionOS.xyz;

                // موج نرم در طول Trail
                float wave =
                    sin(
                        IN.uv.x * _WaveFrequency
                        - _Time.y * _WaveSpeed
                    );

                // در انتهای Trail موج کمتر می‌شود
                float fade = 1.0 - IN.uv.x;

                position.y += wave * _WaveStrength * fade;

                OUT.positionHCS =
                    TransformObjectToHClip(position);

                OUT.uv = IN.uv;
                OUT.color = IN.color;

                return OUT;
            }


            half4 frag(Varyings IN) : SV_Target
            {
                float u = IN.uv.x;
                float v = IN.uv.y;


                // --------------------------------
                // 1. Fade در طول رد قایق
                // --------------------------------

                float lengthFade =
                    pow(saturate(1.0 - u), _FadePower);


                // --------------------------------
                // 2. نرم کردن لبه‌های Trail
                // --------------------------------

                float edgeDistance =
                    min(v, 1.0 - v);

                float edgeFade =
                    smoothstep(
                        0.0,
                        _EdgeSoftness,
                        edgeDistance
                    );


                // --------------------------------
                // 3. موج‌های داخلی
                // --------------------------------

                float wave =
                    sin(
                        u * _WaveFrequency * 1.5
                        - _Time.y * _WaveSpeed
                    );

                wave = wave * 0.5 + 0.5;


                // --------------------------------
                // 4. نوارهای روشن داخل آب
                // --------------------------------

                float highlight =
                    smoothstep(
                        1.0 - _HighlightWidth,
                        1.0,
                        wave
                    );

                highlight *= _HighlightStrength;


                // --------------------------------
                // 5. رنگ اصلی
                // --------------------------------

                float3 finalColor =
                    lerp(
                        _MainColor.rgb,
                        _HighlightColor.rgb,
                        highlight
                    );


                // --------------------------------
                // 6. Alpha نهایی
                // --------------------------------

                float alpha =
                    _Opacity
                    * lengthFade
                    * edgeFade;

                alpha *=
                    lerp(
                        0.75,
                        1.0,
                        wave
                    );

                alpha *= IN.color.a;


                return half4(
                    finalColor,
                    alpha
                );
            }

            ENDHLSL
        }
    }
}