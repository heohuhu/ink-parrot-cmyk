Shader "UI/CMYKMultiplyBlend"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Cull Off
        Lighting Off
        ZWrite Off
        ZTest Always
        Blend DstColor Zero, One OneMinusSrcAlpha   // 곱연산 블렌드 = 감산 혼합(CMY 느낌)

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata { float4 vertex:POSITION; float2 uv:TEXCOORD0; };
            struct v2f { float4 pos:SV_POSITION; float2 uv:TEXCOORD0; };

            sampler2D _MainTex;
            fixed4 _Color;

            v2f vert(appdata v){
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i):SV_Target
            {
                fixed4 tex = tex2D(_MainTex, i.uv) * _Color;
                fixed3 rgb = lerp(fixed3(1,1,1), tex.rgb, tex.a);
                return fixed4(rgb, tex.a); // 알파는 그대로 살려서 리턴
            }
            ENDCG
        }
    }
}