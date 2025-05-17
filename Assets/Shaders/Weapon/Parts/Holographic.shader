Shader "Custom/Holographic"
{
    Properties
    {
        _MainTex ("Dot Texture", 2D) = "white" {}
        _DotColor ("Dot Color", Color) = (1, 0, 0, 1)
        _GlowStrength ("Glow Strength", Range(0, 10)) = 1
        _Thickness ("Dot Thickness", Range(0.5, 3)) = 1
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent+10" }
        LOD 100

        Pass
        {
            ZTest Always
            ZWrite Off
            Blend One One
            Cull Off

            Stencil
            {
                Ref 1
                Comp Equal
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4 _DotColor;
            float _GlowStrength;
            float _Thickness;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texCol = tex2D(_MainTex, i.uv);

                // 텍스처 알파를 두께 비율로 제곱 조절 (중심 확장 느낌)
                float alpha = pow(texCol.a, 1.0 / _Thickness);

                // 알파 컷팅
                clip(alpha - 0.1);

                // 최종 색상 = 도트 색상 * 조정된 알파 * 글로우 세기
                float glow = alpha * _GlowStrength;
                return fixed4(_DotColor.rgb * glow, glow);
            }
            ENDCG
        }
    }
}
