Shader "Unlit/LeftWhiteToRightFade"
{
    Properties
    {
        _Color ("Tint Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _Color;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float SmoothFade(float t)
            {
                return pow(1.0 - t, 2.0); // 부드럽게 감쇠
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = _Color;
                col.rgb = float3(1.0, 1.0, 1.0); // 흰색

                if (i.uv.x <= 0.4)
                {
                    col.a = 1.0; // 왼쪽~중간: 완전 불투명
                }
                else
                {
                    float t = (i.uv.x - 0.4) / 0.5; 
                    col.a = SmoothFade(t);         // 부드럽게 알파 줄임
                }

                return col;
            }
            ENDCG
        }
    }
}
