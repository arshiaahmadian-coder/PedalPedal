Shader "Custom/2DTreeSway"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        [Header(Wind Settings)]
        _WindStrength ("Wind Strength", Range(0, 0.12)) = 0.035
        _WindSpeed ("Wind Speed", Range(0.1, 4)) = 1.1
        _WindFrequency ("Wind Frequency", Range(0.5, 3)) = 1.4
        _WindDirection ("Wind Direction", Range(-1, 1)) = 1

        [Header(Advanced)]
        _HeightPower ("Height Power", Range(0.8, 3)) = 1.7
        _SecondaryStrength ("Secondary Wave", Range(0, 0.6)) = 0.3
        _WorldInfluence ("World Position Influence", Range(0, 2)) = 0.8
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            float _WindStrength;
            float _WindSpeed;
            float _WindFrequency;
            float _WindDirection;
            float _HeightPower;
            float _SecondaryStrength;
            float _WorldInfluence;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(IN);

                float4 vertex = IN.vertex;

                // موقعیت جهانی برای محاسبه موج
                float3 worldPos = mul(unity_ObjectToWorld, vertex).xyz;

                // ارتفاع نسبی از UV (پایدارتر از vertex.y)
                float heightFactor = pow(saturate(IN.texcoord.y), _HeightPower);

                float time = _Time.y * _WindSpeed;

                // موج اصلی + تأثیر موقعیت جهانی
                float wave1 = sin(time + worldPos.x * _WorldInfluence + IN.texcoord.y * _WindFrequency);
                
                // موج دوم (فرکانس متفاوت برای طبیعی‌تر شدن)
                float wave2 = sin(time * 1.63 + worldPos.x * _WorldInfluence * 1.3 + IN.texcoord.y * _WindFrequency * 1.4);

                float windOffset = (wave1 + wave2 * _SecondaryStrength) * _WindStrength * heightFactor * _WindDirection;

                vertex.x += windOffset;

                OUT.vertex = UnityObjectToClipPos(vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;

                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color;
                c.rgb *= c.a;
                return c;
            }
            ENDCG
        }
    }
}